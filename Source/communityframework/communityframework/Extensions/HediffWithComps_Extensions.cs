using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    public static class HediffWithComps_Extensions
    {
        public static void Notify_ThingCrafted(
            this HediffWithComps h,
            ref IEnumerable<Thing> products,
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept = null)
        {
            foreach (HediffComp c in h.comps)
            {
                if (c is HediffComp_OnThingCrafted o)
                {
                    o.Notify_ThingCrafted(
                        ref products,
                        recipeDef,
                        worker,
                        ingredients,
                        dominantIngredient,
                        billGiver,
                        precept);
                }
            }
        }
    }
}
