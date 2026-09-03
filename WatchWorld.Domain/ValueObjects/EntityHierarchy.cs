using System;
using System.Collections.Generic;
using System.Text;

namespace WatchWorld.Domain.ValueObjects
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object? obj)
            => obj is Entity other && Id == other.Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }
    public abstract class Aggregateroot : Entity
    {

    }
}
