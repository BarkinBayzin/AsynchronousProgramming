using AsynchronousProgramming.Infrastructure.Context;
using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Infrastructure.Repositories.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        //Repository içerisinde neden ApplicationDbContext.cs sınıfına ihtiyacımız var? 
        private readonly ApplicationDbContext _dbContext; //ApplicationDbContext.cs sınıfı bizim uygulama tarafındaki veri tabanımızın karşılığıdır.
        protected readonly DbSet<T> _table; // DbSet ise hatırlarsanız ApplicationDbContext sınıf içerisinde tanımladığımız bir yapı idi. O da uygulama tarafında veri tabanınında ki tabloların karşılığıdır.

        //Biz veri tabanı üzerinde herhangi bir CRUD operasyonu yapacağımız zaman teorik olarak düşünürsek veri tabanına  ve onun üzerinden ilgili tabloya erişmemiz gerekmektedir. ORM gereği burada muhakak onların bir karşılığı olmak zorunda.
        public BaseRepository(ApplicationDbContext dbContext)
        {
            //Dependency Injection
            //Eski çalışmalarımızda tam burada yukarıda anlattığım sebepten dolayı ApplicationDbContext.cs sınıfının nesnesini çıkarırdık. Bu örneklem alma işlemi yüzünden repository sınıfları ile ApplicationDbContext.cs sınıf arasında sıkı sıkıya bağlı bir ilişki oluşmaktaydı. Ayrıca memory yönetimi açısından sıkı sıkıya bağlı sınıfların maliyet oluşturulduğunu ve RAM'in Heap alanında yönetilemeyen kaynaklara neden olmaktadır. Sonuç olarak her sınıfın instance'si çıkardığımızda bu nesnelerin yönetimide projelerimiz büyüdükçe sıkıntılar yaşamaktayız. Bu yüzden projelerimizde bu tarz bağımlılıklara sebep olan sınıfları DIP ve IoC prensiplerine de uymak için Dependency Injection deseni kullarak gevşek bağlı bir hale getirmek istiyoruz.

            //Inject ederken 3 farklı yol ile inject edebiliriz:
            //1.Constructor injection
            //2.Custome method injection
            //3.Property ile injection

            //DI bir desendir prensip değil. Hatta DIP ve IoC prensiplerini uygulamamızda bize yardımcı olan bir araçtır. Asp .Net Core Bu prensipleri projelerimizde rahatlıkla kullanmaız için dizayn edilmilmiştir.
            _dbContext = dbContext;
            _table = _dbContext.Set<T>();
        }

        public async Task Add(T entity)
        {
            //await _dbContext.Set<TEntity>().AddAsync(entity);
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression) => await _table.AnyAsync(expression);

        public async Task Delete(T entity)
        {
            entity.Status = Status.Passive;
            entity.DeleteDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<T> GetByDefault(Expression<Func<T, bool>> expression) => await _table.FirstOrDefaultAsync(expression);

        public async Task<List<T>> GetByDefaults(Expression<Func<T, bool>> expression) => await _table.Where(expression).ToListAsync();

        public async Task<T> GetById(int id) => await _table.FindAsync(id);


        public async Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            return _table.Where(expression).ToList();
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderyBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null)
        {
            IQueryable<T> query = _table;

            if (join != null) query = join(query);

            if (orderyBy != null) return await orderyBy(query).Select(select).ToListAsync();

            else return await query.Select(select).ToListAsync();

        }
    }
}
