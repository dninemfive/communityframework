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
            // We don't really need this pawn check right now because this
            // extension is only ever called for the bill worker, but we may as
            // well put it here in case that changes in the future.
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
