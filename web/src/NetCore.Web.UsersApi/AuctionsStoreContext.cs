using Microsoft.EntityFrameworkCore;
using NetCore.Data;

namespace NetCore.Web.UsersApi
{
    public class AuctionsStoreContext : DbContext
    {
        public AuctionsStoreContext(DbContextOptions<AuctionsStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Auction> Auctions { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(c => c.Address);
            modelBuilder.Entity<User>().HasMany<AuctionInfo>(c => c.Auctions);

            modelBuilder.Entity<Auction>().OwnsOne(c => c.Vehicle);
            modelBuilder.Entity<Vehicle>().OwnsOne(c => c.Engine);
        }
    }
}