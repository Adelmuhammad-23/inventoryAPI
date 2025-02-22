using AutoMapper;
using IMS.API.Base;
using IMS.Application.DTOs.AuthenticationDTOs;
using IMS.Application.Responses;
using IMS.Domain.Entities.Identities;
using IMS.Domain.Helper;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : AppControllerBase
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserRepository _userRepository;
        #endregion

        #region constructor
        public AuthenticationController(IMapper mapper,
                                 IAuthenticationRepository authenticationRepository,
                                 SignInManager<User> signInManager,
                                 EmailService emailService,
                                 IUserRepository userRepository,
                                 UserManager<User> userManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticationRepository = authenticationRepository ?? throw new ArgumentNullException();
            _signInManager = signInManager ?? throw new ArgumentNullException();
            _userManager = userManager ?? throw new ArgumentNullException();
            _emailService = emailService;
            _userRepository = userRepository ?? throw new ArgumentNullException();


        }
        #endregion


        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(toEmail))
                    return BadRequest("Recipient email is required.");

                await _emailService.SendEmailAsync(toEmail, subject, body);
                return Ok("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                return StatusCode(500, $"SMTP Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var decodedToken = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully.");
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDTO changePassword)
        {
            //Check if the Id is Exist Or not
            var user = await _userManager.FindByIdAsync(changePassword.Id.ToString());
            //return NotFound
            if (user == null)
                return NotFound("User in not found.");
            var newUser = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

            if (changePassword.NewPassword != changePassword.ConfirmPassword)
                return BadRequest("must newPassword equals confirmPassword.");

            if (!newUser.Succeeded)
                return BadRequest(newUser.Errors.FirstOrDefault().Description);

            return Ok("Change Password is Successfully.");

        }
        [HttpPost("SendCodeToResetPassword")]
        public async Task<Responses<string>> SendCodeToResetPassword([FromForm] SendResetPasswordDTO request)
        {
            var sendCode = await _authenticationRepository.SendResetPasswordCodeAsync(request.Email);
            switch (sendCode)
            {
                case "User Not Found":
                    return NotFound<string>("User Not Found");
                case "Error When send code to Email":
                    return BadRequest<string>("Error When send code to Email");
                case "Success":
                    return Success<string>("Send reset Password code is successfully");
                default:
                    return BadRequest<string>();
            }
        }
        [HttpGet("ConfirmCodeToResetPassword")]
        public async Task<Responses<string>> ConfirmCodeToResetPassword([FromForm] ConfirmResetPasswordDTO request)
        {
            var resetPassword = await _authenticationRepository.ConfirmResetPasswordAsync(request.Email, request.Code);
            switch (resetPassword)
            {
                case "User is not found ":
                    return NotFound<string>("User Not Found");
                case "Invalid Code":
                    return BadRequest<string>("Invalid Code");
                case "Success":
                    return Success<string>("Confirm reset Password code is successfully");
                default:
                    return BadRequest<string>();

            }
        }
        [HttpPost("ResetPassword")]
        public async Task<Responses<string>> ResetPassword([FromForm] ResetPasswordDTo request)
        {
            var resetPassword = await _authenticationRepository.ResetPasswordAsync(request.Email, request.Password);
            switch (resetPassword)
            {
                case "User is not found ":
                    return NotFound<string>("User Not Found");
                case "Failed":
                    return BadRequest<string>("Failed to resetPassword");
                default:
                    return Success<string>("Reset Password is successfully");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<Responses<JwtAuthResult>> RefreshToken([FromBody] RefreshTokenDTO refreshToken)
        {
            var jwtToken = _authenticationRepository.ReadJwtToken(refreshToken.AccessToken);
            var userIdAndExpireDate = await _authenticationRepository.ValidateDetails(jwtToken, refreshToken.AccessToken, refreshToken.RefreshToken);
            switch (userIdAndExpireDate)
            {
                case ("Algorithms is not correct", null):
                    return Unauthorized<JwtAuthResult>("Algorithms Is Not Correct");
                case ("Token is not expired", null):
                    return BadRequest<JwtAuthResult>("Token Is Not Expired");
                case ("Refresh Token is Not Found", null):
                    return NotFound<JwtAuthResult>("Refresh Token Is Not Found");
                case ("Refresh Token is expired", null):
                    return Unauthorized<JwtAuthResult>("Refresh Token Is Expired");
            }

            var (userId, expiryDate) = userIdAndExpireDate;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound<JwtAuthResult>();
            }
            var result = await _authenticationRepository.GetNewRefreshToken(user, jwtToken, expiryDate, refreshToken.RefreshToken);
            return Success(result);
        }
    }
}
