
using System;

namespace Invoicing.Base.Ddd
{
    public abstract class Entity
    {
        private int? _requestedHashCode;
        public string Id { get; protected set; } = Guid.NewGuid().ToString();

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Entity item))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return this.Id == item.Id;
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = Id?.GetHashCode() ?? 0 ^ 31;
            }

            return _requestedHashCode.Value;
        }
    }
}
