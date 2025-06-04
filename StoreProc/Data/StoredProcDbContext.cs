using Microsoft.EntityFrameworkCore;
using StoreProc.Models;

namespace StoreProc.Data
{
    public class StoredProcDbContext : DbContext
    {
        public StoredProcDbContext(DbContextOptions<StoredProcDbContext> options)
            : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
