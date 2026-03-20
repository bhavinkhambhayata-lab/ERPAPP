using ERPAPP.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ERPAPP.Helper
{
    public static class EnumDropDownService
    {
        public static List<BaseDropDown> GetList<TEnum>()
       where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(e => Convert.ToInt32(e) != 0)
                .Select(e => new BaseDropDown
                {
                    Code = e.ToString(),
                    Name = e.ToString()
                })
                .ToList();
        }

        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            if (field == null)
                return value.ToString();

            var attribute = field.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }
    }
}
