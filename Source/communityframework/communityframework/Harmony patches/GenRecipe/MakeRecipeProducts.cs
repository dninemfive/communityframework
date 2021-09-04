using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("MakeRecipeProducts")]
    class MakeRecipeProducts
    {
        public static void Postfix(
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept,
            ref IEnumerable<Thing> __result)
        {
            if (!__result.EnumerableNullOrEmpty())
            {
                worker.Notify_ThingCrafted(
                    ref __result,
                    recipeDef,
                    worker,
                    ingredients,
                    dominantIngredient,
                    billGiver,
                    precept);
                
                // If the billGiver is a ThingWithComps (i.e.
                // Building_WorkTable), then notify it that somthing has been
                // crafted with it.
                if (billGiver is ThingWithComps giverWithComps)
                {
                    giverWithComps.Notify_ThingCrafted(
                        ref __result,
                        recipeDef,
                        worker,
                        ingredients,
                        dominantIngredient,
                        billGiver,
                        precept);
                }

                // Notify the dominant ingredient first because order of
                // execution matters.
                if (dominantIngredient is ThingWithComps dominantWithComps)
                {
                    dominantWithComps.Notify_ThingCrafted(
                        ref __result,
                        recipeDef,
                        worker,
                        ingredients,
                        dominantIngredient,
                        billGiver,
                        precept);
                }

                foreach (Thing thing in ingredients)
                {
                    // We already notified the dominant ingredient, so it would
                    // be a bad idea to notify it again.
                    if (thing == dominantIngredient)
                    {
                        continue;
                    }
                    if (thing is ThingWithComps thingWithComps)
                    {
                        thingWithComps.Notify_ThingCrafted(
                            ref __result,
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
