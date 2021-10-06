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
    /// Additional extensions applied to the <c>HediffWithComps</c> class.
    /// </summary>
    public static class HediffWithComps_Extensions
    {
        /// <summary>
        /// This method is called whenever the <c>Pawn</c> affected by this
        /// <c>Hediff</c> crafts something. It will search for any comps
        /// derived from <c>HediffComp_OnThingCrafted</c> and call
        /// <c>Notify_ThingCrafted.</c>
        /// </summary>
        /// <param name="h">The instance of <c>Hediff</c>.</param>
        /// <param name="products">A collection of <c>Thing</c>s that were
        /// crafted.</param>
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
