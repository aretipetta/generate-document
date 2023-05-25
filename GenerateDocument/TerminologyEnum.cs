﻿using System.ComponentModel;

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
}
