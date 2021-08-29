using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    class ThingCompProperties_OnWearSound : CompProperties
    {
        public SoundDef sound;

        public ThingCompProperties_OnWearSound() =>
            compClass = typeof(ThingComp_OnWearSound);

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
