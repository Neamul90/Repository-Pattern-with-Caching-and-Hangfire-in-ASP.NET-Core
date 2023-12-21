using Caching_Hangfire.Context;
using Caching_Hangfire.Helper;
using Caching_Hangfire.Interface;
using Caching_Hangfire.Model;
using Microsoft.EntityFrameworkCore;

namespace Caching_Hangfire.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customer;
        public CustomerRepository(ApplicationDbContext dbContext, Func<CacheTech, ICacheService> cacheService) : base(dbContext, cacheService)
        {
            _customer = dbContext.Set<Customer>();
        }
    }
}
