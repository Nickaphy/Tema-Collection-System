using System;
using System.Collections.Generic;
using System.Text;
using WatchWorld.Domain.ValueObjects;
using WatchWorld.Domain.Enums;

namespace WatchWorld.Domain.Entities
{
    public class Watches : Aggregateroot
    {
        public string Name { get; private set; }
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

        public Watches(string name, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            Validate();
            Name = name;
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
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(ModelNumber))
                throw new ArgumentException("ModelNumber cannot be null or empty.");
            if (CaseSize <= 0)
                throw new ArgumentException("CaseSize must be greater than zero.");
            if (OriginalPrice < 0)
                throw new ArgumentException("OriginalPrice cannot be negative.");
            if (ReleaseYear.Year > DateTime.Now.Year)
                throw new ArgumentException("ReleaseYear must be before the current year.");
        }
        public void Update(string name, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            Validate();
            Name = name;
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
            Images = images;
        }
        public static Watches Create(string name, string modelNumber, int caseSize, CaseShapeEnum caseShapeEnum, CaseMaterialEnum caseMaterialEnum, MovementTypeEnum movementTypeEnum, string style, decimal originalPrice, GenderEnum genderEnum, DateOnly releaseYear, List<BraceletTypeEnum> braceletTypeEnum, string description, List<HighResImage> images)
        {
            return new Watches(name,
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
        }
    }
}
