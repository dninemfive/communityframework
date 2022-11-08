using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace CF
{
    [StaticConstructorOnStartup]
    public static class ShieldDefaults
    {
        public static readonly Texture2D FullShieldBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        public static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);
        public static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
        public static readonly string InbuiltShieldName = "ShieldInbuilt".Translate().Resolve();
        public static readonly TaggedString PersonalShieldTooltip = "ShieldPersonalTip".Translate();
        public static Command_Action DevAction_Break(CompCustomShield ccs) => new Command_Action()
        {
            defaultLabel = "DEV: Break",
            action = ccs.Break
        };
        public static Command_Action DevAction_Reset(CompCustomShield ccs) => new Command_Action()
        {
            defaultLabel = "DEV: Clear reset",
            action = ccs.Reset
        };
        public static SoundDef AbsorbDamageSound => SoundDefOf.EnergyShield_AbsorbDamage;
        public static FleckDef AbsorbDamageFleck => FleckDefOf.ExplosionFlash;
        public static EffecterDef BreakEffecter => EffecterDefOf.Shield_Break;
        public static FleckDef BreakFleck => FleckDefOf.ExplosionFlash;
        public static SoundDef ResetSound => SoundDefOf.EnergyShield_Reset;
    }
}
