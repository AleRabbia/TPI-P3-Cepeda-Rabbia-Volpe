using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Domain.Enums
{
    public enum CategoryRoom
    {
        [Description("Habitación individual")]
        HabitacionIndividual,
        [Description("Habitación doble estándar (una cama doble)")]
        HabitacionDobleEstandarUnaCamaDoble,
        [Description("Habitación doble estándar (dos camas separadas)")]
        HabitacionDobleEstandarDosCamasSeparadas,
        [Description("Habitación doble deluxe")]
        HabitacionDobleDeluxe,
        [Description("Estudio o apartamento")]
        EstudioOApartamento,
        [Description("Suite júnior")]
        SuiteJunior,
        [Description("Suite ejecutiva")]
        SuiteEjecutiva,
        [Description("Suite presidencial")]
        SuitePresidencial
    }
}
