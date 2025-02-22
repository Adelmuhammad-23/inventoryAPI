using IMS.Domain.Entities.Identities;
using IMS.Domain.Generic;
namespace IMS.Domain.Interfaces
{
    public interface IUserRepository : IGenaricRepository<User>
    {
        public Task<string> AddUserAsync(User user, string Password);
        public Task<User> GetUserByIdAsync(int id);
        public Task<bool> ConfirmEmailAsync(string userId, string token);

    }
}