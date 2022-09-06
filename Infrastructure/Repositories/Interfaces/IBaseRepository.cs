using AsynchronousProgramming.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Infrastructure.Repositories.Interfaces
{
    //Asenkron Programming (Eş zamansız programlama)
    //Bu güne kadar yaptığımız çalışmalarda senkron programlama (eş zamanlı programlama) yapıyorduk. Bu yüzden bir iş (business) yapıldığında kullanıcı arayüzü (UI- User Interface) sadece yapılan bu işe bütün eforunu sarf etmekteydi. Örneğin bir web servisten data çekmek istiyorsunuz ve request attınız, response olarak gelen data'nın listelenmesi işlemine UI thread'di kitlendi. Böylelikle kullancı uygulamanın ona yan tarafta verdiği not tutma bölümünü kullanamaz hale geldi. Senkron programlama burada yetersiz kaldı. Bizim problemimizi yani data listelenirken arayüz  üzerinde not tutuma işini asenkron programming ile yapabiliriz. Asenkron programming aynı anda bir birinden bağımsız olarak işlemler yapmamızı temin edecektir.
    public interface IBaseRepository<T> where T : BaseEntity
    {
        //Bu projede elimizin asenkron programlamaya alışması için bütün methodları asenkton yazacağım. Lakin Create, Update ve Delete işlemleri çok aksi bir business olmadığı sürece asenkton programlanmaz. Bune gerek yoktur. Bunun yanında bizim asıl odaklanmamız gereken nokta Read operasyonlarımızdır.
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression);
        Task<T> GetByDefault(Expression<Func<T, bool>> expression);
        Task<T> GetById(int id);

        // Read Operations
        Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select,
                                                    Expression<Func<T, bool>> where = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                    Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        Task<bool> Any(Expression<Func<T, bool>> expression);

        List<T> Where(Expression<Func<T, bool>> expression);
    }
}
