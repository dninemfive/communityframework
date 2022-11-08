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
    /// <summary>
    /// In 1.4, shield belts were refactored to be comps, and can be set to be ranged with using CompProperties. However, many variables are still
    /// hardcoded, and things can't be overridden, so this class allows that.
    /// </summary>
    [StaticConstructorOnStartup]
    public class CompCustomShield : ThingComp
    {
        // need backing values to use Scribe_Values
        protected float energy;
        public float Energy 
        { 
            get => energy;
            protected set => energy = value;
        }
        protected int ticksToReset;
        public virtual int TicksToReset
        {
            get => ticksToReset;
            protected set => ticksToReset = value;
        }
        protected int lastKeepDisplayTick;
        public virtual int LastKeepDisplayTick
        {
            get => lastKeepDisplayTick;
            protected set => lastKeepDisplayTick = value;
        }
        private Vector3 ImpactAngleVec;
        public virtual int LastAbsorbDamageTick { get; private set; }
        public float MaxDamagedJitterDist => Props.maxDamagedJitterDist;
        public float JitterDurationTicks => Props.jitterDurationTicks;
        public int KeepDisplayingTicks => Props.keepDisplayingTicks;
        public float ApparelScorePerEnergyMax => Props.apparelScorePerEnergyMax;
        public static readonly Material DefaultBubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
        public CompProperties_CustomShield Props => props as CompProperties_CustomShield;
        public float EnergyMax => Props.energyMax ?? parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax);
        public float EnergyGainPerTick => (Props.energyGainPerTick ?? parent.GetStatValue(StatDefOf.EnergyShieldRechargeRate)) / 60f;
        public virtual ShieldState ShieldState
        {
            get
            {
                if (parent is Pawn p && (p.IsCharging() || p.IsSelfShutdown())) return ShieldState.Disabled;
                CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
                if (comp != null && !comp.Awake) return ShieldState.Disabled;
                if (TicksToReset <= 0) return ShieldState.Active;
                return ShieldState.Resetting;
            }
        }
        public virtual Pawn PawnOwner => (parent as Apparel).Wearer ?? parent as Pawn;
        public bool ShouldDisplay
        {
            get
            {
                if(PawnOwner != null)
                {
                    if (!PawnOwner.Spawned || PawnOwner.Dead || PawnOwner.Downed) return false;
                    if (PawnOwner.InAggroMentalState || PawnOwner.Drafted || 
                        (PawnOwner.Faction.HostileTo(Faction.OfPlayer) && !PawnOwner.IsPrisoner) ||
                        (PawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == PawnOwner)) return true;
                }
                if (Find.TickManager.TicksGame < LastKeepDisplayTick + KeepDisplayingTicks) return true;
                return false;
            }
        }
        public bool IsApparel => parent is Apparel;
        public bool IsBuiltIn => !IsApparel;
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref energy, "energy", 0f);
            Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
            Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick", 0);
        }
        public virtual IEnumerable<Gizmo> GetGizmos()
        {
            if((PawnOwner.Faction == Faction.OfPlayer || (parent is Pawn pawn && pawn.RaceProps.IsMechanoid)) && Find.Selector.SingleSelectedThing == PawnOwner)
            {
                yield return new Gizmo_EnergyShieldStatus()
                {
                    shield = this
                };
            }
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetWornGizmosExtra()) yield return item;
        }        
    }
}
