using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    public static class ThingWithComps_Extensions
    {
        public static void Notify_ThingCrafted(
            this ThingWithComps thing,
            ref IEnumerable<Thing> products,
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept = null)
        {
            foreach (ThingComp c in thing.AllComps)
            {
                if (c is ThingComp_OnThingCrafted o)
                {
                    // This whole code block is just one function call with a
                    // lot of parameters.
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

            // Only Pawns have Hediffs, so only check for a HediffSet if we're
            // looking at a pawn.
            if (thing is Pawn pawn)
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    if (hediff is HediffWithComps withComps)
                    {
                        withComps.Notify_ThingCrafted(
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
}
