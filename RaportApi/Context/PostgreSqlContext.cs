using Microsoft.EntityFrameworkCore;
using RaportApi.Entity;

namespace RaportApi.Context
{
    public class PostgreSqlContext: DbContext
    {
        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
        {
        }

        public DbSet<Raport> Raports { get; set; }
    }
}