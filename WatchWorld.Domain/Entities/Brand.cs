using WatchWorld.Domain.Service;
using WatchWorld.Domain.ValueObjects;

namespace WatchWorld.Domain.Entities
{
    public class Brand : Aggregateroot
    {
        public string Name { get; private set; }
        public int FoundingYear { get; private set; }
        public string CountryOfOrigin { get; private set; }
        public string? ParentGroup { get; private set; }
        public string LogoUrl { get; private set; }
        public string WebsiteUrl { get; private set; }
        public string FounderName { get; private set; }
        public string FounderDescriptionNote { get; private set; }
        public string? OriginDescriptionNote { get; private set; }
        public string BrandDescriptionNote { get; private set; }
        public string WatchStyleDescriptionNote { get; private set; }
        public bool IsActive { get; private set; }

        private Brand() { }

        private Brand(string name, int foundingYear, string countryOfOrigin, string? parentGroup, string logoUrl, string websiteUrl, string founderName, string founderDescriptionNote, string? originDescriptionNote, string brandDescriptionNote, string watchStyleDescriptionNote, bool isActive)
        {
            Name = name;
            FoundingYear = foundingYear;
            CountryOfOrigin = countryOfOrigin;
            ParentGroup = parentGroup;
            LogoUrl = logoUrl;
            WebsiteUrl = websiteUrl;
            FounderName = founderName;
            FounderDescriptionNote = founderDescriptionNote;
            OriginDescriptionNote = originDescriptionNote;
            BrandDescriptionNote = brandDescriptionNote;
            WatchStyleDescriptionNote = watchStyleDescriptionNote;
        }

        public static Brand Create(string name, int foundingYear, string countryOfOrigin, string? parentGroup, string logoUrl, string websiteUrl, string founderName, string founderDescriptionNote, string? originDescriptionNote, string brandDescriptionNote, string watchStyleDescriptionNote, bool isActive)
        {
            var brand = new Brand(name, foundingYear, countryOfOrigin, parentGroup, logoUrl, websiteUrl, founderName, founderDescriptionNote, originDescriptionNote, brandDescriptionNote, watchStyleDescriptionNote, isActive);
            Validate(name, foundingYear, countryOfOrigin, founderName, founderDescriptionNote, brandDescriptionNote, watchStyleDescriptionNote, isActive);
            return brand;
        }

        public static Brand Update(Brand existingBrand, string? parentGroup, string logoUrl, string websiteUrl, string founderDescriptionNote, string? originDescriptionNote, string brandDescriptionNote, string watchStyleDescriptionNote, bool isActive)
        {
            Validate(existingBrand.Name, existingBrand.FoundingYear, existingBrand.CountryOfOrigin, existingBrand.FounderName, founderDescriptionNote, brandDescriptionNote, watchStyleDescriptionNote, isActive);
            return new Brand(
                name: existingBrand.Name,
                foundingYear: existingBrand.FoundingYear,
                countryOfOrigin: existingBrand.CountryOfOrigin,
                parentGroup: parentGroup,
                logoUrl: logoUrl,
                websiteUrl: websiteUrl,
                founderName: existingBrand.FounderName,
                founderDescriptionNote: founderDescriptionNote,
                originDescriptionNote: originDescriptionNote,
                brandDescriptionNote: brandDescriptionNote,
                watchStyleDescriptionNote: watchStyleDescriptionNote,
                isActive: isActive
            );
        }

        public static void Validate(string name, int foundingYear, string countryOfOrigin, string founderName, string founderDescriptionNote, string brandDescriptionNote, string watchStyleDescriptionNote, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new UserInvalidInputException("Brand name cannot be null or empty.");
            if (foundingYear < 1500 || foundingYear > DateTime.Now.Year)
                throw new UserInvalidInputException("Founding year must be between 1500 and the current year.");
            if (string.IsNullOrWhiteSpace(countryOfOrigin))
                throw new UserInvalidInputException("Country of origin cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(founderName))
                throw new UserInvalidInputException("Founder name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(founderDescriptionNote))
                throw new UserInvalidInputException("Founder description note cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(brandDescriptionNote))
                throw new UserInvalidInputException("Brand description note cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(watchStyleDescriptionNote))
                throw new UserInvalidInputException("Watch style description note cannot be null or empty.");
            if (isActive != true && isActive != false)
                throw new UserInvalidInputException("IsActive must be a boolean value.");
        }
    } 
}
