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
            }
        }
    }
}
