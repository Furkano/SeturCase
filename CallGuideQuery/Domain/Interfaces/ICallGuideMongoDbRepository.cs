using Domain.Entity;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;

namespace Domain.Interfaces
{
    public interface ICallGuideMongoDbRepository<T> where T :BaseEntity
    {
        IEnumerable<T> Where(Expression<Func<T, bool>> expression);
        Task<bool> CreateManyAsync(IEnumerable<T> models);
        Task<bool> CreateAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(int id);

        Task<T> FindById(int id);
        
    }
}