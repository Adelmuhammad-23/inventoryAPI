using IMS.Domain.Generic;
using IMS.Infrastructure.DbContext;

namespace IMS.Infrastructure.GenericImplementation
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
