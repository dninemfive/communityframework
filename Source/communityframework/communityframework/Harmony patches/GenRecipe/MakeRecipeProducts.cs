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
    /// <summary>
    /// Patch on <c>GenRecipe.MakeRecipeProducts</c>.
    /// </summary>
    [HarmonyPatch(typeof(GenRecipe))]
    [HarmonyPatch("MakeRecipeProducts")]
    class MakeRecipeProducts
    {
        /// <summary>
        /// Postfix that runs whenever the products of a recipe are created.
        /// This patch calls the <c>Notify_ThingCrafted</c> extension on the
        /// <c>Thing</c>s involved in the recipe.
        /// </summary>
        /// <param name="recipeDef">The <c>Def</c> of the crafting recipe.
        /// </param>
        /// <param name="worker">The <c>Pawn</c> that is doing the crafting.
        /// </param>
        /// <param name="ingredients">A list of <c>Thing</c>s that will be
        /// consumed by the recipe</param>
        /// <param name="dominantIngredient">The <c>Thing</c> marked as the
        /// recipe's dominant ingredient.</param>
        /// <param name="billGiver">The <c>IBillGiver</c> (usually a workbench
        /// for crafting recipes, and a <c>Pawn</c> for surgical recipes) that
        /// the recipe is being performed on.</param>
        /// <param name="precept">The style given to the resulting
        /// <c>Thing</c>s.</param>
        /// <param name="__result">The result of the original method call, a
        /// list of <c>Thing</c>s produced by the recipe.</param>
        public static void Postfix(
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept,
            ref IEnumerable<Thing> __result)
        {
            // If nothing was actually crafted, stop here. This will most
            // likely prevent surgery recipes from being affected, but that's
            // something that would need to be fully fleshed out later anyways.
            if (__result.EnumerableNullOrEmpty())
            {
                return;
            }
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
