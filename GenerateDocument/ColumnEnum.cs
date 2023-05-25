using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GenerateDocument
{
    public enum ColumnEnum
    {
        [Description("ΜΥΙΚΗ ΜΑΖΑ")]
        MUSCLE_GROUPS = 1,
        [Description("ΠΕΡΙΓΡΑΦΗ")]
        DESCRIPTION = 2,
        [Description("ΕΞΟΠΛΙΣΜΟΣ")]
        EQUIPEMENT = 3,
        [Description("ΣΕΤ")]
        SET = 4,
        [Description("ΕΠΑΝΑΛ.")]
        REPS = 5,
        [Description("ΔΙΑΛ.")]
        BREAK = 6,
        [Description("ΠΑΡΑΤΗΡΗΣΕΙΣ")]
        NOTE = 7
    }


    public static class ColumnProcess
    {
        // gets the greek term of a column that is displayed on table as header
        public static string columnEnumToGreek(int index)
        {
            FieldInfo fi = ((ColumnEnum)index).GetType().GetField(((ColumnEnum)index).ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.First().Description;
        }
    }
}
