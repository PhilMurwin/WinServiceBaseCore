using System;
using System.Reflection;

namespace WinServiceBaseCore.Infrastructure
{
    public abstract class BaseProcessSettings
    {
        public abstract string ConfigurationSectionName { get; }
        public bool Enabled { get; set; }
        public int Frequency { get; set; }


        public virtual bool EqualsByValue(BaseProcessSettings other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            foreach (var prop in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var thisVal = prop.GetValue(this);
                var otherVal = prop.GetValue(other);

                if (!Equals(thisVal, otherVal))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj) => obj is BaseProcessSettings other && EqualsByValue(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var prop in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(this);
                hash.Add(value);
            }
            return hash.ToHashCode();
        }

        public static bool operator ==(BaseProcessSettings left, BaseProcessSettings right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(BaseProcessSettings left, BaseProcessSettings right)
        {
            return !(left == right);
        }
    }
}
