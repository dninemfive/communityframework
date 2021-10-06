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
    /// Base <c>HediffComp</c> for <c>Hediff</c>s that are supposed to react
    /// whenever the affected pawn crafts something
    /// </summary>
    public abstract class HediffComp_OnThingCrafted : HediffComp
    {
        /// <summary>
        /// This method is called whenever the affected <c>Pawn</c> crafts an
        /// item or items. The items crafted are passed by reference, and can
        /// therefore be modified.
        /// </summary>
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
        public abstract void Notify_ThingCrafted(
            ref IEnumerable<Thing> products,
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept = null);
    }
}
