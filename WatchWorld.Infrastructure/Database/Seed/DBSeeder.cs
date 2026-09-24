namespace WatchWorld.Infrastructure.Database.Seed
{
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using System.Runtime.Serialization;
    using WatchWorld.Domain.Entities;
    using WatchWorld.Domain.Enums;
    using WatchWorld.Domain.ValueObjects;
    using WatchWorld.Domain.ValueObjects.ManyToMany;
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Prevent duplicate seeding
            if (await context.Users.AnyAsync()) return;

            // Ensure a reference Watch exists from your existing watch seed
            var watchBase = await context.Watchlist.FirstOrDefaultAsync();
            if (watchBase == null)
            {
                throw new InvalidOperationException("No Watches found in database. Seed the Watchlist aggregate before running DbSeeder.");
            }

            // ==========================================
            // 1. USERS
            // ==========================================
            var alice = CreateDomainObject<User>(new()
            {
                ["FirstName"] = "Alice",
                ["LastName"] = "Nielsen",
                ["PhoneNumber"] = "+45 2012 3456",
                ["Email"] = "alice.nielsen@watchworld.dk",
                ["Address"] = "Vestergade 14, 2. th",
                ["City"] = "Odense",
                ["Note"] = "Vintage Omega & Rolex collector. Very reliable borrower.",
                ["Password"] = "$2a$12$eImiTXuWVxfM37uY4JANjO5E/Qd/./v7YdZ2Y2q8.1M22.GfK1234",
                ["IsAdmin"] = false
            });

            var bob = CreateDomainObject<User>(new()
            {
                ["FirstName"] = "Bob",
                ["LastName"] = "Møller",
                ["PhoneNumber"] = "+45 3045 6789",
                ["Email"] = "bob.moeller@watchworld.dk",
                ["Address"] = "Strandvejen 88",
                ["City"] = "Copenhagen",
                ["Note"] = "Lover of luxury sports chronographs.",
                ["Password"] = "$2a$12$eImiTXuWVxfM37uY4JANjO5E/Qd/./v7YdZ2Y2q8.1M22.GfK5678",
                ["IsAdmin"] = false
            });

            var charlie = CreateDomainObject<User>(new()
            {
                ["FirstName"] = "Charlie",
                ["LastName"] = "Hansen",
                ["PhoneNumber"] = "+45 4099 8877",
                ["Email"] = "admin@watchworld.dk",
                ["Address"] = "Åboulevarden 12",
                ["City"] = "Aarhus",
                ["Note"] = "Platform Administrator.",
                ["Password"] = "$2a$12$eImiTXuWVxfM37uY4JANjO5E/Qd/./v7YdZ2Y2q8.1M22.GfK9999",
                ["IsAdmin"] = true
            });

            await context.Users.AddRangeAsync(alice, bob, charlie);
            await context.SaveChangesAsync();

            // ==========================================
            // 2. USER RATINGS
            // ==========================================
            var rating1 = CreateDomainObject<UserRating>(new()
            {
                ["RatedToUserId"] = alice.Id,
                ["RatedByUserId"] = bob.Id,
                ["RatingAmount"] = 5,
                ["Description"] = "Alice returned my watch in pristine condition and right on time!"
            });

            var rating2 = CreateDomainObject<UserRating>(new()
            {
                ["RatedToUserId"] = bob.Id,
                ["RatedByUserId"] = alice.Id,
                ["RatingAmount"] = 5,
                ["Description"] = "Smooth experience. Watch came carefully packed with full documentation."
            });

            await context.UserRatings.AddRangeAsync(rating1, rating2);

            // ==========================================
            // 3. HIGH RES IMAGES
            // ==========================================
            var img1 = CreateDomainObject<HighResImage>(new()
            {
                ["Url"] = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?q=80&w=1200",
                ["Width"] = 1920,
                ["Height"] = 1080
            });

            var img2 = CreateDomainObject<HighResImage>(new()
            {
                ["Url"] = "https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?q=80&w=1200",
                ["Width"] = 1920,
                ["Height"] = 1080
            });

            var img3 = CreateDomainObject<HighResImage>(new()
            {
                ["Url"] = "https://images.unsplash.com/photo-1547996160-0127452d70b8?q=80&w=1200",
                ["Width"] = 1920,
                ["Height"] = 1080
            });

            await context.HighResImages.AddRangeAsync(img1, img2, img3);

            // ==========================================
            // 4. INDIVIDUAL WATCHES
            // ==========================================
            var individualWatch1 = CreateDomainObject<IndividualWatch>(new()
            {
                ["SpecificWatch"] = watchBase,
                ["WearGrade"] = GetEnumValue<WearGradeEnum>(0), // First enum value (e.g. Mint/New)
                ["Age"] = 1,
                ["Note"] = "Includes box and stamped warranty card. Zero scratches.",
                ["EstimatedValue"] = 14500.00m,
                ["Picture"] = new List<HighResImage> { img1, img2 }
            });

            var individualWatch2 = CreateDomainObject<IndividualWatch>(new()
            {
                ["SpecificWatch"] = watchBase,
                ["WearGrade"] = GetEnumValue<WearGradeEnum>(1), // Second enum value (e.g. Good)
                ["Age"] = 3,
                ["Note"] = "Daily wearer with minor hairline scuffs on the clasp.",
                ["EstimatedValue"] = 11200.00m,
                ["Picture"] = new List<HighResImage> { img3 }
            });

            await context.IndividualWatches.AddRangeAsync(individualWatch1, individualWatch2);

            // ==========================================
            // 5. LISTINGS
            // ==========================================
            var listing1 = CreateDomainObject<Listing>(new()
            {
                ["BorrowableWatch"] = individualWatch1,
                ["PricePerDay"] = 65.00m
            });

            var listing2 = CreateDomainObject<Listing>(new()
            {
                ["BorrowableWatch"] = individualWatch2,
                ["PricePerDay"] = 40.00m
            });

            await context.Listings.AddRangeAsync(listing1, listing2);

            // ==========================================
            // 6. TIME SLOTS & BORROWS
            // ==========================================
            var timeSlotPast = CreateTimeSlot(DateTime.UtcNow.AddDays(-14), DateTime.UtcNow.AddDays(-7));
            var timeSlotActive = CreateTimeSlot(DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(5));

            var borrowCompleted = CreateDomainObject<Borrow>(new()
            {
                ["BorrowedByUserId"] = bob.Id,
                ["BorrowedFromUserId"] = alice.Id,
                ["BorrowTimeSlot"] = timeSlotPast,
                ["Status"] = ParseEnum<BorrowStatus>("Completed")
            });

            var borrowActive = CreateDomainObject<Borrow>(new()
            {
                ["BorrowedByUserId"] = alice.Id,
                ["BorrowedFromUserId"] = bob.Id,
                ["BorrowTimeSlot"] = timeSlotActive,
                ["Status"] = ParseEnum<BorrowStatus>("Active")
            });

            await context.Borrows.AddRangeAsync(borrowCompleted, borrowActive);

            // ==========================================
            // 7. RECORD / JOIN TABLES
            // ==========================================
            // Active Borrows
            var activeBorrowRecord = CreateDomainObject<ActiveBorrows>(new()
            {
                ["ListingId"] = listing2.Id,
                ["BorrowId"] = borrowActive.Id
            });

            // User Ownerships
            var ownershipAlice = CreateDomainObject<UserOwnsWatch>(new()
            {
                ["UserId"] = alice.Id,
                ["WatchId"] = watchBase.Id
            });

            var ownershipBob = CreateDomainObject<UserOwnsWatch>(new()
            {
                ["UserId"] = bob.Id,
                ["WatchId"] = watchBase.Id
            });

            // Watch Borrows History / Active
            var watchBorrowActive = CreateDomainObject<WatchBorrow>(new()
            {
                ["UserId"] = alice.Id,
                ["IndividualWatchId"] = individualWatch2.Id
            });

            var watchBorrowPast = CreateDomainObject<WatchBorrow>(new()
            {
                ["UserId"] = bob.Id,
                ["IndividualWatchId"] = individualWatch1.Id
            });

            await context.ActiveBorrows.AddAsync(activeBorrowRecord);
            await context.UserOwnsWatches.AddRangeAsync(ownershipAlice, ownershipBob);
            await context.WatchBorrows.AddRangeAsync(watchBorrowActive, watchBorrowPast);

            await context.SaveChangesAsync();
        }

        #region Reflection Infrastructure for Private Setters and Records

        private static T CreateDomainObject<T>(Dictionary<string, object?> properties) where T : class
        {
            // Bypasses missing default constructor or private constructors
            var instance = (T)FormatterServices.GetUninitializedObject(typeof(T));

            // Auto-assign Id if class inherits from Entity
            var idProp = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (idProp != null && idProp.PropertyType == typeof(Guid))
            {
                SetMemberValue(instance, "Id", Guid.NewGuid());
            }

            foreach (var (key, value) in properties)
            {
                SetMemberValue(instance, key, value);
            }

            return instance;
        }

        private static void SetMemberValue<T>(T instance, string name, object? value)
        {
            var type = instance!.GetType();

            // 1. Try public/non-public property setter
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(instance, value);
                return;
            }

            // 2. Fallback to C# compiler-generated backing field or convention field
            var fieldNames = new[]
            {
            $"<{name}>k__BackingField",                 // Standard auto-property or record field
            $"_{char.ToLower(name[0])}{name[1..]}",      // _propertyName field
            $"m_{char.ToLower(name[0])}{name[1..]}",     // m_propertyName field
            name                                         // Exact field match
        };

            foreach (var fieldName in fieldNames)
            {
                var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(instance, value);
                    return;
                }
            }
        }

        private static TimeSlot CreateTimeSlot(DateTime from, DateTime to)
        {
            var timeSlot = (TimeSlot)FormatterServices.GetUninitializedObject(typeof(TimeSlot));
            SetMemberValue(timeSlot, "From", (DateTimeOffset)from);
            SetMemberValue(timeSlot, "To", (DateTimeOffset)to);
            return timeSlot;
        }

        private static TEnum GetEnumValue<TEnum>(int index) where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            return values[Math.Min(index, values.Length - 1)];
        }

        private static TEnum ParseEnum<TEnum>(string name) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(name, true, out var result) ? result : default;
        }

        #endregion
    }
}
