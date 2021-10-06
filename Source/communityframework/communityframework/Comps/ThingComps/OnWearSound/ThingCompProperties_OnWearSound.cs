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
    /// The properties that control a respective <c>ThingComp_OnWearSound</c>. 
    /// When applied to an apparel item, it can play a sound whenever the item
    /// is put on or taken off.
    /// </summary>
    class ThingCompProperties_OnWearSound : CompProperties
    {
        /// <summary>
        /// The sound to play after the item is put on.
        /// </summary>
        public SoundDef wornSound = null;
        /// <summary>
        /// The sound to play after the item is taken off.
        /// </summary>
        public SoundDef removedSound = null;

        /// <summary>
        /// Default constructor, sets <c>compClass</c> to
        /// <c>ThingComp_OnWearSound</c>.
        /// </summary>
        public ThingCompProperties_OnWearSound() =>
            compClass = typeof(ThingComp_OnWearSound);

        /// <summary>
        /// Override of method used to report errors made by modders using this
        /// <c>ThingComp</c>. This will inform the modder if they have applied
        /// the <c>ThingComp</c> to something other than an apparel item.
        /// </summary>
        /// <param name="parentDef">The <c>ThingDef</c> of the item that the
        /// <c>ThingComp</c> was applied to.</param>
        /// <returns>A full list of config errors.</returns>
        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            List<string> errors =
                new List<string>(base.ConfigErrors(parentDef));
            if (!parentDef.IsApparel)
            {
                errors.Add("ThingCompProperties_OnWearSound applied to " +
                    "ThingDef " + parentDef.label + ", which is not an " +
                    "apparel item.");
            }
            return errors.AsEnumerable();
        }
    }
}
