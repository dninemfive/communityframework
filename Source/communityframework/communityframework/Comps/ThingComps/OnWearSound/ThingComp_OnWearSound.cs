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
    /// <summary>
    /// The functional component of <c>ThingCompProperties_OnWearSound</c>. 
    /// When applied to an apparel item, it can play a sound whenever the item
    /// is put on or taken off.
    /// </summary>
    class ThingComp_OnWearSound : ThingComp
    {
        /// <summary>
        /// Pre-cast reference to this comp's respective <c>CompProperties</c>.
        /// </summary>
        public ThingCompProperties_OnWearSound Props =>
            (ThingCompProperties_OnWearSound)props;

        /// <summary>
        /// Runs after the parent <c>Thing</c> is put on by a <c>Pawn</c>.
        /// Plays the <c>SoundDef</c> stored in <c>Props.wornSound</c>.
        /// </summary>
        /// <param name="pawn">The <c>Pawn</c> putting on the item.</param>
        public override void Notify_Equipped(Pawn pawn)
        {
            if (Props.wornSound != null && pawn.Map == Find.CurrentMap)
            {
                Props.wornSound.PlayOneShot(
                    new TargetInfo(pawn.Position, pawn.Map, false));
            }
            base.Notify_Equipped(pawn);
        }

        /// <summary>
        /// Runs after the parent <c>Thing</c> is taken off of a <c>Pawn</c>.
        /// Plays the <c>SoundDef</c> stored in <c>Props.removedSound</c>.
        /// </summary>
        /// <param name="pawn">The <c>Pawn</c> that the item was removed from.
        /// </param>
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
