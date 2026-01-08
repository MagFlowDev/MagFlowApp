using System.ComponentModel.DataAnnotations;

namespace MagFlow.Shared.Attributes.ValidationAttributes
{
    public sealed class RequiredKeyAttribute : RequiredAttribute
    {
        public RequiredKeyAttribute(string key) => ErrorMessage = key;
    }


}
