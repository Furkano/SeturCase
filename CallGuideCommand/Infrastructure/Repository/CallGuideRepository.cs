using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repository
{
    public class CallGuideRepository<T> : ICallGuideRepository<T> where T : BaseEntity
    {
        private PostgreSqlContext postgreSqlContext;

        public CallGuideRepository(PostgreSqlContext _postgreSqlContext)
        {
            postgreSqlContext = _postgreSqlContext ?? throw new ArgumentNullException(nameof(_postgreSqlContext));
        }
        public async Task<T> Create(T entity)
        {
            var result = await postgreSqlContext.Set<T>()
                .AddAsync(entity);
            await postgreSqlContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Boolean> Delete(T entity)
        {
            postgreSqlContext.Set<T>().Remove(entity);
            return (await postgreSqlContext.SaveChangesAsync()) > 0;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return postgreSqlContext.Set<T>().Where(expression);
        }
    }
}