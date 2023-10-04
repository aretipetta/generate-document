using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GenerateDocument
{
    public enum TerminologyEnum
    {
        [Description("ΣΤΗΘΟΣ")]
        CHEST = 1,
        [Description("ΠΛΑΤΗ")]
        BACK = 2,
        [Description("ΩΜΟΙ")]
        SHOULDERS = 3,
        [Description("ΔΙΚΕΦΑΛΟΙ")]
        BICEPS = 4,
        [Description("ΤΡΙΚΕΦΑΛΟΙ")]
        TRICEPS = 5,
        [Description("ΚΟΙΛΙΑΚΟΙ")]
        ABS = 6,
        [Description("ΡΑΧΙΑΙΟΙ")]
        DORSALS = 7,
        [Description("ΠΟΔΙΑ")]
        LEGS_E = 8
    }

    public static class MuscleGroup
    {
        // gets the greek term of a column that is displayed on table as header
        public static string muscleGroupToGreek(int index)
        {
            FieldInfo fi = ((TerminologyEnum)index).GetType().GetField(((TerminologyEnum)index).ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.First().Description;
        }
    }
}
