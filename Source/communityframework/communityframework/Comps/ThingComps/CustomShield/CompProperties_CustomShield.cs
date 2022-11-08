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
    public class CompProperties_CustomShield : CompProperties_Shield
    {
        /// <summary>
        /// The maximum distance the bubble will move when damaged.
        /// </summary>
        public float maxDamagedJitterDist = 0.05f;
        /// <summary>
        /// The duration, in ticks, of the jitter effect when damaged.
        /// </summary>
        public int jitterDurationTicks = 8;
        /// <summary>
        /// The number of ticks the shield will keep displaying after combat.
        /// </summary>
        public int keepDisplayingTicks = 1000;
        /// <summary>
        /// The amount of apparel score the shield adds based on its maximum energy.
        /// </summary>
        public float apparelScorePerEnergyMax = 0.25f;
        /// <summary>
        /// The maximum energy the shield can have. If unset, will use the <see cref="StatDefOf.EnergyShieldEnergyMax">corresponding Stat</see>, as per vanilla.
        /// </summary>
        public float? energyMax;
        /// <summary>
        /// The amount of energy the shield regains per tick when recharging. If unset, will use the 
        /// <see cref="StatDefOf.EnergyShieldRechargeRate">corresponding StatDef</see>, as per vanilla.
        /// </summary>
        public float? energyGainPerTick = null;
        public string bubbleTex = null;
        /// <summary>
        /// Class to organize gizmo props together in XML.
        /// </summary>
        public class GizmoProps
        {
            /// <summary>
            /// The texture to apply to the full portion of the bar. Checked first; if not set, <see cref="fullBarColor">fullBarColor</see>
            /// is tried, and if that is also null, <see cref="ShieldDefaults.FullShieldBarTex">the default color</see> is used..
            /// </summary>
            public string fullBarTex = null;
            /// <summary>
            /// The solid color to apply to the full portion of the bar. Checked second, and therefore overridden by <see cref="fullBarTex">fullBarTex</see>
            /// if that is set. If this is not set, defaults to the <see cref="ShieldDefaults.FullShieldBarTex">the default color</see> is used.
            /// </summary>
            public Color? fullBarColor = null;
            /// <summary>
            /// The texture to apply to the empty portion of the bar. Checked first; if not set, <see cref="emptyBarColor">fullBarColor</see>
            /// is tried, and if that is also null, <see cref="ShieldDefaults.EmptyShieldBarTex">the default color</see> is used..
            /// </summary>
            public string emptyBarTex = null;
            /// <summary>
            /// The solid color to apply to the empty portion of the bar. Checked second, and therefore overridden by <see cref="emptyBarTex">fullBarTex</see>
            /// if that is set. If this is not set, defaults to the <see cref="ShieldDefaults.EmptyShieldBarTex">the default color</see> is used.
            /// </summary>
            public Color? emptyBarColor = null;
            /// <summary>
            /// The width of the shield gizmo.
            /// </summary>
            public float gizmoWidth = 140f;
            /// <summary>
            /// A custom shield name, to override the thing's name if this comp is on an Apparel item, or the default "builtin" string if it's a built-in shield.
            /// </summary>
            public string customNameKey = null;
            /// <summary>
            /// A custom tooltip, to override the default shield tooltip.
            /// </summary>
            public string customTooltipKey = null;
        }
        /// <summary>
        /// The properties of the gizmo associated with this shield. See the description for <see cref="GizmoProps">GizmoProps</see> for more details.
        /// </summary>
        public GizmoProps gizmoProperties = null;
        /// <summary>
        /// <see cref="DamageDef">DamageDef</see>s which cause the shield to instantly break. By default, this is only EMP damage.
        /// </summary>
        public List<DamageDef> instantBreakDamageDefs = new List<DamageDef>() { DamageDefOf.EMP };
        /// <summary>
        /// <see cref="DamageDef"/>s which are absorbed by this shield. By default, the game does not use <c>DamageDef</c>s specifically to check absorption,
        /// so the list is empty.
        /// </summary>
        public List<DamageDef> extraAbsorbDamageDefs = new List<DamageDef>();
        /// <summary>
        /// Whether the shield should absorb ranged damage; true by default.
        /// </summary>
        public bool absorbRangedDamage = true;
        /// <summary>
        /// Whether the shield should absorb explosive damage; true by default.
        /// </summary>
        public bool absorbExplosiveDamage = true;
        /// <summary>
        /// Whether the shield should absorb all damage. CAUTION: this includes positive <see cref="DamageDef"/>s, such as <!-- todo: find and list examples-->(idk).
        /// </summary>
        public bool absorbAllDamage = false;
        /// <summary>
        /// The <see cref="SoundDef"/> to play when the shield absorbs damage.
        /// </summary>
        public SoundDef absorbDamageSound = null;
        /// <summary>
        /// The <see cref="FleckDef"/> to throw when the shield absorbs damage.
        /// </summary>
        public FleckDef absorbDamageFleck = null;
        /// <summary>
        /// The <see cref="EffecterDef"/> applied when the shield breaks.
        /// </summary>
        public EffecterDef breakEffecter = null;
        /// <summary>
        /// The <see cref="FleckDef"/> to throw when the shield breaks.
        /// </summary>
        public FleckDef breakFleck = null;
        /// <summary>
        /// The <see cref="SoundDef"/> to play when the shield resets.
        /// </summary>
        public SoundDef resetSound = null;
        /// <summary>
        /// The <see cref="Verb"/>s to potentially block. By default, none are directly set.
        /// </summary>
        public List<Verb> verbsToBlock = new List<Verb>();
        public CompProperties_CustomShield()
        {
            compClass = typeof(CompCustomShield);
        }
    }
}
