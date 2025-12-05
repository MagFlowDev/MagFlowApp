using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagFlow.Shared.Models.Enumerators
{
    public abstract class CodeEnumeration<Code> : IComparable where Code : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected CodeEnumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null))
                    .Cast<T>();

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }


        public int CompareTo(object? obj)
        {
            if (obj is null)
                return Id.CompareTo(null);
            return Id.CompareTo(((Enumeration)obj).Id);
        }
    }
}
