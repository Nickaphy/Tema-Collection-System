
using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;

namespace WatchWorld.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //Aggregates and Entities
        public DbSet<Borrow> Borrows => Set<Borrow>();
        public DbSet<Listing> Listings => Set<Listing>();
        public DbSet<HighResImage> HighResImages => Set<HighResImage>();
        public DbSet<IndividualWatch> IndividualWatches => Set<IndividualWatch>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Watches> Watchlist => Set<Watches>();
        public DbSet<UserRating> UserRatings => Set<UserRating>();

        //Many to Many Relationships
        public DbSet<UserOwnsWatch> UserOwnsWatches => Set<UserOwnsWatch>();
        public DbSet<ActiveBorrows> ActiveBorrows => Set<ActiveBorrows>();
        public DbSet<WatchBorrow> WatchBorrows => Set<WatchBorrow>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Borrow>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Rating)
                .WithOne()
                .HasForeignKey(r => r.RatedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRating>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.RatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IndividualWatch>()
                .HasOne(iw => iw.SpecificWatch)
                .WithMany()
                .HasForeignKey("WatchesId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IndividualWatch>()
                .HasMany(iw => iw.Picture)
                .WithOne()
                .HasForeignKey("IndividualWatchId")
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
