using DeliveryStorage.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryStorage.Database.Data;

public class DatabaseContext : DbContext
{
    public DbSet<BoxDb> Boxes { get; set; }
    public DbSet<PalletDb> Pallets { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}