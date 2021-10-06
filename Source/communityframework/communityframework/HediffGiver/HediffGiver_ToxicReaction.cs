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
    /// <c>HediffGiver</c> used to allow certain races to react differently to
    /// toxic fallout, instead of always developing toxic buildup.
    /// </summary>
    class HediffGiver_ToxicReaction : HediffGiver
    {
        /// <summary>
        /// The hediff to apply instead of toxic buildup.
        /// </summary>
        public HediffDef hediffDef;

        /// <summary>
        /// Run whenever any hediff is added. Checks to see if the added
        /// hediff is toxic buildup, which is replaced with the hediff in 
        /// <c>HediffDef</c>.
        /// </summary>
        /// <param name="pawn">The <c>Pawn</c> that <c>hediff</c> was applied
        /// to.</param>
        /// <param name="hediff">The <c>Hediff</c> applied to <c>pawn</c>.
        /// </param>
        /// <returns>Always the base method's return value.</returns>
        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (hediff.def == HediffDefOf.ToxicBuildup)
            {
                HealthUtility.AdjustSeverity(pawn, hediffDef, hediff.Severity);
                pawn.health.RemoveHediff(hediff);
            }
            return base.OnHediffAdded(pawn, hediff);
        }
    }
}
