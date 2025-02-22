using IMS.Domain.Entities.Identities;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace IMS.Infrastructure.Repositories
{
    public class UserRepository : GenaricRepository<User>, IUserRepository
    {
        #region Fields
        private readonly DbSet<User> _user;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructor
        public UserRepository(UserManager<User> userManager, ApplicationDbContext dbContext) : base(dbContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _user = dbContext.Set<User>();
        }

        #endregion

        #region Methods
        public async Task<string> AddUserAsync(User user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(password)) return "PasswordCannotBeEmpty";

            // Check if email or username already exists
            var existingUser = await CheckUserExistenceAsync(user);
            if (existingUser != null) return existingUser;

            // Create user
            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                return createResult.Errors.FirstOrDefault()?.Description ?? "UnknownError";

            // Assign default role
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return roleResult.Errors.FirstOrDefault()?.Description ?? "FailedToAssignRole";


            return "Success";
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        #endregion
        #region Method Helper
        private async Task<string> CheckUserExistenceAsync(User user)
        {
            if (await _userManager.FindByEmailAsync(user.Email) != null)
                return "EmailIsExist";

            if (await _userManager.FindByNameAsync(user.UserName) != null)
                return "UserNameIsExist";

            return null;
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
        #endregion

    }
}