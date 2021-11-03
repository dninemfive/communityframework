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
    /// Additional extensions applied to the <c>ThingWithComps</c> class.
    /// </summary>
    public static class ThingWithComps_Extensions
    {
        /// <summary>
        /// This method is called whenever this <c>Thing</c> is involved in a
        /// crafting recipe. It will search for any comps derived from
        /// <c>ThingfComp_OnThingCrafted</c> and call
        /// <c>Notify_ThingCrafted</c>. If this <c>Thing</c> is also a
        /// <c>Pawn</c>, then it will check the pawn's <c>hediffSet</c> for any
        /// <c>HediffWithComps</c>'s and call the extension
        /// <c>Notify_ThingCrafted</c>.
        /// </summary>
        /// <param name="thing">The instance of <c>ThingWithComps</c>.</param>
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
                if (c is IThingComp_OnThingCrafted o)
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
