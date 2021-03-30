using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RaportApi.Interfaces
{
    public interface IPostgreSqlRaportRepository<T>
    {
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<Boolean> Delete(T entity);
    }
}