using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Resposes;
using Domain.Entity;

namespace Application.Interfaces
{
    public interface IMongoRepository
    {
        IEnumerable<CallGuide> Where(Expression<Func<CallGuide, bool>> expression);
        Task<bool> CreateManyAsync(IEnumerable<CallGuide> models);
        Task<bool> CreateAsync(CallGuide model);
        Task<bool> UpdateAsync(CallGuide model);
        Task<bool> DeleteAsync(int id);
        Task<CallGuide> FindById(int id);
        List<RaportItem> GetRaportItem(int userId);         
    }
}