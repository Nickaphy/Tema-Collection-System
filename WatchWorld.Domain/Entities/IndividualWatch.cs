using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Text;
using WatchWorld.Domain.ValueObjects;

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
    }
}
