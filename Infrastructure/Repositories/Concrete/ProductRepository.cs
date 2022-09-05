using AsynchronousProgramming.Infrastructure.Context;
using AsynchronousProgramming.Models.Entities.Concrete;

namespace AsynchronousProgramming.Infrastructure.Repositories.Concrete
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
