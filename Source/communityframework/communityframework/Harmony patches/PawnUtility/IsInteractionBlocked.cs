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
    /// Patch on <c>PawnUtility.IsInteractionBlocked</c>.
    /// </summary>
    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("IsInteractionBlocked")]
    class IsInteractionBlocked
    {
        /// <summary>
        /// Patch on method <c>IsInteractionBlocked</c>. This patch provides
        /// the functionality of the <c>InteractionFilter</c> class.
        /// </summary>
        /// <param name="__result">The result of the original method call;
        /// <c>true</c> if the interaction was blocked by something else.
        /// </param>
        /// <param name="pawn">The <c>Pawn</c> being checked.</param>
        /// <param name="interaction">The <c>InteractionDef</c> being checked.
        /// </param>
        /// <param name="isInitiator"><c>true</c> if <c>pawn</c> started the
        /// interaction.</param>
        /// <param name="isRandom"><c>true</c> if the interaction occured
        /// randomly.</param>
        public static void postfix(
            ref bool __result,
            Pawn pawn,
            InteractionDef interaction,
            bool isInitiator,
            bool isRandom)
        {
            InteractionFilter filter =
                pawn.def.GetModExtension<InteractionFilter>();

            if (filter != null && 
                filter.IsInteractionDisabled(interaction, isInitiator))
            {
                __result = true;
            }
        }
    }
}
