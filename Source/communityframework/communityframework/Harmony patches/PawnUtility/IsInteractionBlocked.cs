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
    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch("IsInteractionBlocked")]
    class IsInteractionBlocked
    {
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
