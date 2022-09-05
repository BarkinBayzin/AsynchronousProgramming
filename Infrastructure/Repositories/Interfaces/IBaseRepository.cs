using AsynchronousProgramming.Models.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Infrastructure.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression);
        Task<T> GetByDefault(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);
        Task<bool> Any(Expression<Func<T, bool>> expression);

        List<T> Where(Expression<Func<T, bool>> expression);


        //Task<List<TEntity>> GetFilteredList<TEntity>(Expression<Func<TEntity, bool>> select,
        //                                             Expression<Func<TEntity, bool>> where = null,
        //                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderyBy = null);
    }
}
