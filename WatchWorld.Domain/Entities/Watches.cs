using WatchWorld.Domain.ValueObjects;
using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.Entities
{
    public class Watches : Aggregateroot
    {
        public string Name { get; private set; }
        public Guid BrandId { get; private set; }
        public string ModelNumber { get; private set; }
        public int CaseSize { get; private set; }
        public CaseShapeEnum CaseShapeEnum { get; private set; }
        public CaseMaterialEnum CaseMaterialEnum { get; private set; }
        public MovementTypeEnum MovementTypeEnum { get; private set; }
        public string Style { get; private set; }
        public decimal OriginalPrice { get; private set; }
        public GenderEnum GenderEnum { get; private set; }
        public DateOnly ReleaseYear { get; private set; }
        public List<BraceletTypeEnum> BraceletTypeEnum { get; private set; }
        public string Description { get; private set; }
        public List<HighResImage> Images { get; private set; }



        private Watches() { }

        private Watches(string name, Guid brandId, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            Name = name;
            BrandId = brandId;
            ModelNumber = modelNumber;
            CaseSize = caseSize;
            CaseShapeEnum = caseShapeEnum;
            CaseMaterialEnum = caseMaterialEnum;
            MovementTypeEnum = movementTypeEnum;
            Style = style;
            OriginalPrice = originalPrice;
            GenderEnum = genderEnum;
            ReleaseYear = releaseYear;
            BraceletTypeEnum = braceletTypeEnum ?? new List<BraceletTypeEnum>();
            Description = description;
            Images = images ?? new List<HighResImage>();
        }
        public static void Validate(string Name, Guid BrandId, string ModelNumber, int CaseSize, decimal OriginalPrice, DateOnly ReleaseYear)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new UserInvalidInputException("Name cannot be null or empty.");
            if (BrandId == Guid.Empty)
                throw new UserInvalidInputException("Brand cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(ModelNumber))
                throw new UserInvalidInputException("ModelNumber cannot be null or empty.");
            if (CaseSize <= 0)
                throw new UserInvalidInputException("CaseSize must be greater than zero.");
            if (OriginalPrice < 0)
                throw new UserInvalidInputException("OriginalPrice cannot be negative.");
            if (ReleaseYear.Year > DateTime.Now.Year)
                throw new UserInvalidInputException("ReleaseYear must be before the current year.");
        }
        public static Watches Update(string name, Guid brandId, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            Validate(name, brandId, modelNumber, caseSize, originalPrice, releaseYear);
            return new Watches
            {
                Name = name,
                BrandId = brandId,
                ModelNumber = modelNumber,
                CaseSize = caseSize,
                CaseShapeEnum = caseShapeEnum,
                CaseMaterialEnum = caseMaterialEnum,
                MovementTypeEnum = movementTypeEnum,
                Style = style,
                OriginalPrice = originalPrice,
                GenderEnum = genderEnum,
                ReleaseYear = releaseYear,
                BraceletTypeEnum = braceletTypeEnum ?? new List<BraceletTypeEnum>(),
                Description = description,
                Images = images
            };
        }
        public static Watches Create(string name, Guid brandId, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            
            var watch = new Watches(name,
                                brandId,
                                modelNumber,
                                caseSize,
                                caseShapeEnum,
                                caseMaterialEnum,
                                movementTypeEnum,
                                style,
                                originalPrice,
                                genderEnum,
                                releaseYear,
                                braceletTypeEnum,
                                description,
                                images);
            Validate(watch.Name, watch.BrandId, watch.ModelNumber, watch.CaseSize, watch.OriginalPrice, watch.ReleaseYear);
            return watch;
        }
    }
}
