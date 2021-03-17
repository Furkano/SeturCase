using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICallGuideRepository<T>
    {
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        Task<T> Create(T entity);
        Task<Boolean> Delete(T entity);

    }
}