using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    public static class Pawn_Extensions
    {
        public static bool IsRace(this Pawn pawn, List<ThingDef> races)
        {
            return races.Contains(pawn.def);
        }

        public static bool IsRace(this Pawn pawn, ThingDef race)
        {
            return pawn.IsRace(new List<ThingDef>() { race });
        }
    }
}
