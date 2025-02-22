using IMS.Domain.Entities.Identities;
using IMS.Domain.Interfaces;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.GenericImplementation;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Repositories
{
    public class UserRefreshTokenRepository : GenaricRepository<UserRefreshToken>, IUserRefreshTokenRepository
    {
        #region Fields
        private readonly DbSet<UserRefreshToken> _userRefreshTokens;
        #endregion

        #region Constructors
        public UserRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _userRefreshTokens = dbContext.Set<UserRefreshToken>();
        }
        #endregion



    }
}
