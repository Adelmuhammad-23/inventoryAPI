using IMS.Domain.Entities.Identities;
using IMS.Domain.Generic;
using IMS.Domain.Helper;
using System.IdentityModel.Tokens.Jwt;

namespace IMS.Domain.Interfaces
{
    public interface IAuthenticationRepository : IGenaricRepository<UserRefreshToken>
    {
        public Task<JwtAuthResult> GetJwtToken(User user);
        public Task<string> SendResetPasswordCodeAsync(string email);
        public Task<string> ConfirmResetPasswordAsync(string email, string code);
        public Task<string> ResetPasswordAsync(string email, string Password);
        public Task<JwtAuthResult> GetNewRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
    }
}
