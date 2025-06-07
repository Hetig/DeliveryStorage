using Microsoft.EntityFrameworkCore;
    
namespace DeliveryStorage.Database.Data
{
    public class DatabaseContext : DbContext
    {
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
    }
}
