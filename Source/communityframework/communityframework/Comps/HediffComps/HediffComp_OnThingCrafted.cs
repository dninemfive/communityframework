using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    public abstract class HediffComp_OnThingCrafted : HediffComp
    {
        public virtual void Notify_ThingCrafted(
            ref IEnumerable<Thing> Products,
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept = null)
        {
        }
    }
}
