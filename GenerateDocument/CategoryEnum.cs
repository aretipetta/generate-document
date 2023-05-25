using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GenerateDocument
{
    public enum CategoryEnum
    {
        [Description("ΠΑΝΩ ΜΕΡΟΣ")]
        UPPER_BODY = 1,
        [Description("ΠΟΔΙΑ")]
        LEGS = 2,
        [Description("ΣΥΝΔΥΑΣΜΟΣ")]
        MIX = 3
    }
    
    public static class CategoryProcess
    {
        // gets the greek term of muscle group
        public static string categoryEnumToGreek(CategoryEnum category)
        {
            FieldInfo fi = category.GetType().GetField(category.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.First().Description;
        }
    }
}
