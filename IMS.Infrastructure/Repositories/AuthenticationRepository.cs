using IMS.Domain.Entities.Identities;
using IMS.Domain.Helper;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.ExternalServices;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IMS.Infrastructure.Repositories
{
    public class AuthenticationRepository : GenaricRepository<UserRefreshToken>, IAuthenticationRepository
    {
        private readonly DbSet<UserRefreshToken> _userRefreshTokens;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly ConcurrentDictionary<string, RefreshToken> _userRefreshToken;
        private readonly EmailService _emailService;
        private readonly UserManager<User> _userManager;

        public AuthenticationRepository(JwtSettings jwtSettings, EmailService emailService, UserManager<User> userManager, IUserRefreshTokenRepository userRefreshTokenRepository, ApplicationDbContext dbContext) : base(dbContext)

        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _userRefreshTokens = dbContext.Set<UserRefreshToken>();
            _userRefreshToken = new ConcurrentDictionary<string, RefreshToken>();
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _emailService = emailService;
        }
        public async Task<JwtAuthResult> GetJwtToken(User user)
        {
            var (jwtToken, accessToken) = await GetJWTToken(user);
            var refreshToken = GetRefreshToken(user.UserName);

            var refreshTokenResult = new UserRefreshToken
            {
                Token = accessToken,
                RefreshToken = refreshToken.TokenString,
                IsRevoked = false,
                IsUsed = true,
                AddedTime = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserId = user.Id,
                JwtId = jwtToken.Id,
            };

            //add this data in UserRefreshTokenTable in database
            await _userRefreshTokens.AddAsync(refreshTokenResult);
            await _context.SaveChangesAsync();

            var response = new JwtAuthResult();
            response.AccessToken = accessToken;
            response.RefreshToken = refreshToken;
            return response;
        }

        public async Task<string> SendResetPasswordCodeAsync(string email)
        {
            var transact = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return "User Not Found";
                //Generate random number to send in email
                Random generator = new Random();
                string randomNumber = generator.Next(0, 10000).ToString("D6");
                //update user in database
                user.Code = randomNumber;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                    return "Error When send code to Email";
                var massage = $"{randomNumber} is your password reset code";

                await _emailService.SendEmailAsync(user.Email, massage, "ResetPassword");
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "Failed";
            }
        }
        public async Task<string> ConfirmResetPasswordAsync(string email, string code)
        {
            //user
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "User is not found ";
            //code in database 
            //check code is equal or not
            if (user.Code != code)
                return "Invalid Code";
            return "Success";
        }

        public async Task<string> ResetPasswordAsync(string email, string Password)
        {
            var transact = _context.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return "User is not found ";

                var removeOldPassword = await _userManager.RemovePasswordAsync(user);
                var updatePassword = await _userManager.AddPasswordAsync(user, Password);
                await transact.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transact.RollbackAsync();
                return "Failed";
            }
        }

        #region Claims Functions
        public async Task<List<Claim>> GetClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }
        #endregion


        #region JWT Token Functions  For Help
        // using table to return more than one of types like string and JwtSecurityToken
        private async Task<(JwtSecurityToken, string)> GetJWTToken(User user)
        {
            var claims = await GetClaims(user);
            var jwtToken = new JwtSecurityToken(
              _jwtSettings.Issuer,
              _jwtSettings.Audience,
                claims,
              expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature));
            //token
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            await _context.SaveChangesAsync();

            return (jwtToken, accessToken);
        }


        public async Task<JwtAuthResult> GetNewRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {

            var (jwtSecurityToken, newToken) = await GetJWTToken(user);
            #region Generate New Refresh Token
            var response = new JwtAuthResult();
            //new AccessToken
            response.AccessToken = newToken;
            //new Refresh Token
            var refreshTokenResult = new RefreshToken();
            refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.UserName)).Value;
            refreshTokenResult.TokenString = refreshToken;
            refreshTokenResult.ExpierAt = (DateTime)expiryDate;
            response.RefreshToken = refreshTokenResult;

            return response;
            #endregion
        }

        #endregion


        #region Refresh Token Functions for Help
        private RefreshToken GetRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken
            {
                ExpierAt = DateTime.Now.AddMonths(_jwtSettings.RefreshTokenExpireDate),
                TokenString = GeneratRefreshToken(),
                UserName = userName
            };
            //if refreshtoken is exist => update if not Add
            _userRefreshToken.AddOrUpdate(refreshToken.TokenString, refreshToken, (s, r) => refreshToken);
            return refreshToken;
        }
        private string GeneratRefreshToken()
        {
            var rondamNumber = new byte[32];
            var rondamNumberGenerated = RandomNumberGenerator.Create();
            rondamNumberGenerated.GetBytes(rondamNumber);
            return Convert.ToBase64String(rondamNumber);
        }
        #endregion


        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException(nameof(accessToken));

            var handler = new JwtSecurityTokenHandler();

            var response = handler.ReadJwtToken(accessToken);
            return response;

        }

        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken)
        {
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                return ("Algorithms is not correct", null);
            }
            // validate Token
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return ("Token is not expired", null);
            }

            //Get User Id
            var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == nameof(UserClaimModel.Id)).Value;
            //Get UserGetTableAsTracking
            var userRefreshToken = await _userRefreshTokenRepository.GetTableNoTracking()
                                             .FirstOrDefaultAsync(x => x.Token == accessToken &&
                                                                     x.RefreshToken == refreshToken &&
                                                                     x.UserId == int.Parse(userId));

            if (userRefreshToken == null)
            {
                return ("Refresh Token is Not Found", null);
            }

            // validate Refresh Token
            if (userRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                userRefreshToken.IsRevoked = true;
                userRefreshToken.IsUsed = false;
                await _userRefreshTokenRepository.UpdatAsync(userRefreshToken);
                return ("Refresh Token is expired", null);
            }
            var expirydate = userRefreshToken.ExpiryDate;
            return (userId, expirydate);
        }

    }
}