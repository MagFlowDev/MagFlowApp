using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Enumerators
{
    public abstract class Enumeration<I> : IComparable where I : IComparable
    {
        public string Name { get; private set; }

        public I Id { get; private set; }

        protected Enumeration(I id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration<I> =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration<I> otherValue)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
                return Id.CompareTo(null);
            return Id.CompareTo(((Enumeration<I>)obj).Id);
        }
    }
}
