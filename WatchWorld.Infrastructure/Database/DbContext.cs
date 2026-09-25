
using Microsoft.EntityFrameworkCore;
using WatchWorld.Domain.Entities;
using WatchWorld.Domain.ValueObjects.ManyToMany;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WatchWorld.Infrastructure.Database
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Aggregates and Entities
        public DbSet<Borrow> Borrows => Set<Borrow>();
        public DbSet<Listing> Listings => Set<Listing>();
        public DbSet<HighResImage> HighResImages => Set<HighResImage>();
        public DbSet<IndividualWatch> IndividualWatches => Set<IndividualWatch>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Watches> Watchlist => Set<Watches>();
        public DbSet<UserRating> UserRatings => Set<UserRating>();

        // Many to Many / Value Records
        public DbSet<UserOwnsWatch> UserOwnsWatches => Set<UserOwnsWatch>();
        public DbSet<ActiveBorrows> ActiveBorrows => Set<ActiveBorrows>();
        public DbSet<WatchBorrow> WatchBorrows => Set<WatchBorrow>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Discovers and applies all IEntityTypeConfiguration classes in the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }

    #region Entity Framework Configurations

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Rating)
                .WithOne()
                .HasForeignKey(r => r.RatedTargetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class UserRatingConfiguration : IEntityTypeConfiguration<UserRating>
    {
        public void Configure(EntityTypeBuilder<UserRating> builder)
        {
            builder.HasKey(ur => ur.Id);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(ur => ur.RatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class IndividualWatchConfiguration : IEntityTypeConfiguration<IndividualWatch>
    {
        public void Configure(EntityTypeBuilder<IndividualWatch> builder)
        {
            builder.HasKey(iw => iw.Id);

            builder.HasOne(iw => iw.SpecificWatch)
                .WithMany()
                .HasForeignKey("WatchesId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(iw => iw.Picture)
                .WithOne()
                .HasForeignKey("IndividualWatchId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(iw => iw.EstimatedValue)
                .HasPrecision(10, 2);
        }
    }

    public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.HasKey(b => b.Id);

            // Flattens TimeSlot into the Borrow table
            builder.OwnsOne(b => b.BorrowTimeSlot);
        }
    }

    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasOne(l => l.BorrowableWatch)
                .WithMany()
                .HasForeignKey("IndividualWatchId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(l => l.PricePerDay)
                .HasPrecision(10, 2);
        }
    }

    public class WatchesConfiguration : IEntityTypeConfiguration<Watches>
    {
        public void Configure(EntityTypeBuilder<Watches> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.OriginalPrice)
                .HasPrecision(10, 2);
        }
    }

    public class RecordConfigurations :
        IEntityTypeConfiguration<UserOwnsWatch>,
        IEntityTypeConfiguration<ActiveBorrows>,
        IEntityTypeConfiguration<WatchBorrow>
    {
        // Composite keys, not HasNoKey(): these are real join rows the seeder/repositories
        // insert into, and EF's keyless entities are read-only (can't be tracked/inserted).
        public void Configure(EntityTypeBuilder<UserOwnsWatch> builder) => builder.HasKey(x => new { x.UserId, x.WatchId });
        public void Configure(EntityTypeBuilder<ActiveBorrows> builder) => builder.HasKey(x => new { x.ListingId, x.BorrowId });
        public void Configure(EntityTypeBuilder<WatchBorrow> builder) => builder.HasKey(x => new { x.UserId, x.IndividualWatchId });
    }

    #endregion
}
