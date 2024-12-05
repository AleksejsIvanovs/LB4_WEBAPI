using Microsoft.EntityFrameworkCore;

namespace Lab2.DataAccess
{
    public class HouseDbContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Garage> Garages { get; set; }

        public HouseDbContext(DbContextOptions<HouseDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\GIT_HUB\LB4_WEBAPI\LB2_DATAACCESS\LAB2.DATAACCESS\HOUSEDB.MDF;Integrated Security=True");
            }
        }
    }
}
