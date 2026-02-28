using ERPAPP.Models;

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
    }
}
