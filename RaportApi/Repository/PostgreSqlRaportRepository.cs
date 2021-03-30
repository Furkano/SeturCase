using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RaportApi.Context;
using RaportApi.Interfaces;

namespace RaportApi.Repository
{
    public class PostgreSqlRaportRepository<T> : IPostgreSqlRaportRepository<T> where T : class
    {
        private PostgreSqlContext postgreSqlContext;

        public PostgreSqlRaportRepository(PostgreSqlContext _postgreSqlContext)
        {
            postgreSqlContext = _postgreSqlContext ?? throw new ArgumentNullException(nameof(_postgreSqlContext));
        }
        public virtual async Task<T> Create(T entity)
        {
            var result = await postgreSqlContext.Set<T>()
                .AddAsync(entity);
            await postgreSqlContext.SaveChangesAsync();
            return result.Entity;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            postgreSqlContext.Set<T>().Remove(entity);
            return (await postgreSqlContext.SaveChangesAsync()) > 0;
        }

        public virtual async Task<T> Update(T entity)
        {
            var result= postgreSqlContext.Set<T>().Update(entity);
            await postgreSqlContext.SaveChangesAsync();
            return result.Entity;
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return postgreSqlContext.Set<T>().Where(expression);
        }
    }
}