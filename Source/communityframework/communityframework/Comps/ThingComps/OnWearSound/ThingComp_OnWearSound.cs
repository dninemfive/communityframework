using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using RimWorld;

namespace CF
{
    class ThingComp_OnWearSound : ThingComp
    {
        ThingCompProperties_OnWearSound Props =>
            (ThingCompProperties_OnWearSound)props;

        public override void Notify_Equipped(Pawn pawn)
        {
            if (Props.wornSound != null && pawn.Map == Find.CurrentMap)
            {
                Props.wornSound.PlayOneShot(
                    new TargetInfo(pawn.Position, pawn.Map, false));
            }
            base.Notify_Equipped(pawn);
        }

        public override void Notify_Unequipped(Pawn pawn)
        {
            if (Props.removedSound != null && pawn.Map == Find.CurrentMap)
            {
                Props.removedSound.PlayOneShot(
                    new TargetInfo(pawn.Position, pawn.Map, false));
            }
            base.Notify_Unequipped(pawn);
        }
    }
}
