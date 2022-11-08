﻿using RimWorld;
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
        public float? energyGainPerTick;
        public string bubbleTex = null;
        public Color? barColor = null;
        public string barTex = null;
        public Color? emptyBarColor = null;
        public string emptyBarTex = null;
        public float gizmoWidth = 140f;
        public string customNameKey = null;
        public string customTooltipKey = null;
        public List<DamageDef> instantBreakDamageDefs = new List<DamageDef>() { DamageDefOf.EMP };
        public List<DamageDef> extraAbsorbDamageDefs = new List<DamageDef>();
        public bool absorbRangedDamage = true;
        public bool absorbExplosiveDamage = true;
        public bool absorbAllDamage = false;
        public SoundDef absorbDamageSound = null;
        public FleckDef absorbDamageFleck = null;
        public EffecterDef breakEffecter = null;
        public FleckDef breakFleck = null;
        public SoundDef resetSound = null;
        public List<Verb> verbsToBlock = new List<Verb>();
        public CompProperties_CustomShield()
        {
            compClass = typeof(CompCustomShield);
        }
    }
}
