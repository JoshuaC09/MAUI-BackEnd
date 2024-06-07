using Microsoft.EntityFrameworkCore;
using WebApplication1.DatabaseContext;

namespace WebApplication2
{
    public class MyDbContextFactory
    {
        public MyDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
