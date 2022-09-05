using AsynchronousProgramming.Infrastructure.Context;
using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Infrastructure.Repositories.Concrete
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _table;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<TEntity>();
        }

        public async Task Add(TEntity entity)
        {
            //await _dbContext.Set<TEntity>().AddAsync(entity);
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> expression) => await _table.AnyAsync(expression);

        public async Task Delete(TEntity entity)
        {
            entity.Status = Status.Passive;
            entity.DeleteDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<TEntity> GetByDefault(Expression<Func<TEntity, bool>> expression) => await _table.FirstOrDefaultAsync(expression);

        public async Task<List<TEntity>> GetByDefaults(Expression<Func<TEntity, bool>> expression) => await _table.Where(expression).ToListAsync();

        public async Task<TEntity> GetById(int id) => await _table.FindAsync(id);

        //public async Task<List<TEntity>> GetFilteredList<TEntity>(Expression<Func<TEntity, bool>> select, Expression<Func<TEntity, bool>> where = default, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = default)
        //{
        //    IQueryable<TEntity> query = (IQueryable<TEntity>)_table;

        //    if (where != null)
        //    {
        //        query = query.Where(where);
        //    }

        //    if (orderyBy != null)
        //    {
        //        return await orderyBy(query).Select(select).ToListAsync();
        //    }
        //}

        public async Task Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public List<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _table.Where(expression).ToList();
        }
    }
}
