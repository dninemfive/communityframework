using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// Additional extensions applied to the <c>Pawn</c> class.
    /// </summary>
    public static class Pawn_Extensions
    {
        /// <summary>
        /// Used to determine if a <c>Pawn</c> has one of multiple races.
        /// </summary>
        /// <param name="pawn">The <c>Pawn</c> to check.</param>
        /// <param name="races">A list of races to check for.</param>
        /// <returns><c>true</c> if <c>pawn</c>'s race is contained in
        /// <c>races</c>.</returns>
        public static bool IsRace(this Pawn pawn, List<ThingDef> races)
        {
            return races.Contains(pawn.def);
        }

        /// <summary>
        /// Used to determine if a <c>Pawn</c> is of a specific race.
        /// </summary>
        /// <param name="pawn">The <c>Pawn</c> to check.</param>
        /// <param name="race">The race to check for.</param>
        /// <returns><c>true</c> if <c>pawn</c>'s race is equal to <c>race</c>.
        /// </returns>
        public static bool IsRace(this Pawn pawn, ThingDef race)
        {
            return pawn.IsRace(new List<ThingDef>() { race });
        }
    }
}
