using AsynchronousProgramming.Infrastructure.Context;
using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.Entities.Concrete;

namespace AsynchronousProgramming.Infrastructure.Repositories.Concrete
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
