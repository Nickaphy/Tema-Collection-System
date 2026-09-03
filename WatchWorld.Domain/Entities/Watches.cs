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

    }
}
