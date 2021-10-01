using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    class HediffGiver_ToxicReaction : HediffGiver
    {
        public HediffDef hediffDef;

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
