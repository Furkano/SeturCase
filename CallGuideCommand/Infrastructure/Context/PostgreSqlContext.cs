using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class PostgreSqlContext: DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }

        public DbSet<CallGuide> CallGuides { get; set; }
    }
}