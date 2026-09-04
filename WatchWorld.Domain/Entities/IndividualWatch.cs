using WatchWorld.Domain.ValueObjects;
using WatchWorld.Domain.Enums;
using WatchWorld.Domain.Service;

namespace WatchWorld.Domain.Entities
{
    public class IndividualWatch : Aggregateroot
    {
        public Watches SpecificWatch { get; private set; }
        public WearGradeEnum WearGrade { get; private set; }
        public int Age { get; private set; } 
        public string Note { get; private set; }
        public decimal EstimatedValue { get; private set; }
        public List<HighResImage> Picture { get; private set; }


        private IndividualWatch() { }

        private IndividualWatch(Watches specificWatch, WearGradeEnum wearGrade, int age, string note, decimal estimatedValue, List<HighResImage> picture)
        {
            SpecificWatch = specificWatch;
            WearGrade = wearGrade;
            Age = age;
            Note = note;
            EstimatedValue = estimatedValue;
            Picture = picture ?? new List<HighResImage>();
        }

        public static IndividualWatch Create(Watches specificWatch, WearGradeEnum wearGrade, int age, string note, decimal estimatedValue, List<HighResImage> picture)
        {
            var IndividualWatch = new IndividualWatch(specificWatch, wearGrade, age, note, estimatedValue, picture);
            Validate(specificWatch, wearGrade, age, estimatedValue);
            return IndividualWatch;
        }

        public static IndividualWatch Update(IndividualWatch existingWatch, Watches specificWatch, WearGradeEnum wearGrade, int age, string note, decimal estimatedValue, List<HighResImage> picture)
        {
            Validate(specificWatch, wearGrade, age, estimatedValue);
            existingWatch.SpecificWatch = specificWatch;
            existingWatch.WearGrade = wearGrade;
            existingWatch.Age = age;
            existingWatch.Note = note;
            existingWatch.EstimatedValue = estimatedValue;
            existingWatch.Picture = picture ?? new List<HighResImage>();
            return existingWatch;
        }

        public static void Validate(Watches specificWatch, WearGradeEnum wearGrade, int age, decimal estimatedValue)
        {
            if (specificWatch is null)
                throw new UserInvalidInputException("Der skal vælges en ur model");
            if (age < 0)
                throw new UserInvalidInputException("Et ur kan ikke være mindre end 0 år");
            if (estimatedValue < 0)
                throw new UserInvalidInputException("Et ur kan ikke have en negativ værdi");
            if (!Enum.IsDefined(typeof(WearGradeEnum), wearGrade))
                throw new UserInvalidInputException("Et ur skal have et gyldigt slidniveau");
            if (specificWatch.ReleaseYear.Year < (DateTime.Now.Year - age))
                throw new UserInvalidInputException("Et ur kan ikke være ældre end modellets udgivelsesår");
        }
    }
}