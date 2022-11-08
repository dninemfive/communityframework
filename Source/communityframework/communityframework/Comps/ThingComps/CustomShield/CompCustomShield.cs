using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace CF
{
    /// <summary>
    /// In 1.4, shield belts were refactored to be comps, and can be set to be ranged with using CompProperties. However, many variables are still
    /// hardcoded, and things can't be overridden, so this class allows that.
    /// </summary>
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
        public virtual int LastAbsorbDamageTick { get; protected set; }
        public float MaxDamagedJitterDist => Props.maxDamagedJitterDist;
        public float JitterDurationTicks => Props.jitterDurationTicks;
        public int KeepDisplayingTicks => Props.keepDisplayingTicks;
        public float ApparelScorePerEnergyMax => Props.apparelScorePerEnergyMax;        
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
            // these bools just to make the if statement more readable
            bool ownerIsFactionPawn = PawnOwner.Faction == Faction.OfPlayer;
            bool parentIsMechanoid = parent is Pawn pawn && pawn.RaceProps.IsMechanoid;
            if((ownerIsFactionPawn || parentIsMechanoid) && Find.Selector.SingleSelectedThing == PawnOwner)
            {
                yield return new Gizmo_CustomShieldStatus(this);
            }
            // Moved here from CompGetWornGizmosExtra because they should still work for built-in shields
            if (!DebugSettings.ShowDevGizmos) yield break;
            yield return ShieldDefaults.DevAction_Break(this);
            if (ShieldState is ShieldState.Resetting) yield return ShieldDefaults.DevAction_Reset(this);
        }
        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetWornGizmosExtra()) yield return item;
            if (IsApparel) foreach (Gizmo gizmo in GetGizmos()) yield return gizmo;            
        }        
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra()) yield return item;
            if (!IsBuiltIn) yield break;
            foreach (Gizmo gizmo in GetGizmos()) yield return gizmo;
        }
        public override float CompGetSpecialApparelScoreOffset() => EnergyMax * ApparelScorePerEnergyMax;
        public override void CompTick()
        {
            base.CompTick();
            if (PawnOwner is null) energy = 0f;
            else if(ShieldState is ShieldState.Resetting)
            {
                if ((--ticksToReset) <= 0) Reset();
            }
            else if(ShieldState is ShieldState.Active)
            {
                energy += EnergyGainPerTick;
                if (energy > EnergyMax) energy = EnergyMax;
            }
        }
        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            absorbed = false;
            if (ShieldState != ShieldState.Active) return;
            if (Props.instantBreakDamageDefs.Contains(dinfo.Def)) Break();
            else if(Props.absorbAllDamage || Props.extraAbsorbDamageDefs.Contains(dinfo.Def) ||
                (Props.absorbRangedDamage && dinfo.Def.isRanged) || (Props.absorbExplosiveDamage && dinfo.Def.isExplosive))
            {
                energy -= dinfo.Amount * Props.energyLossPerDamage;
                if (energy < 0f) Break();
                else AbsorbedDamage(dinfo);
                absorbed = true;
            }
        }
        public virtual void KeepDisplaying() => lastKeepDisplayTick = Find.TickManager.TicksGame;
        public SoundDef AbsorbDamageSound => Props.absorbDamageSound ?? ShieldDefaults.AbsorbDamageSound;
        public FleckDef AbsorbDamageFleck => Props.absorbDamageFleck ?? ShieldDefaults.AbsorbDamageFleck;
        protected virtual void AbsorbedDamage(DamageInfo dinfo)
        {
            // todo: this will throw an error if PawnOwner is null. Shouldn't (?) be a problem with the base game code,
            // but if someone overrides some functions here and not this one it could cause unintended errors.
            AbsorbDamageSound.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
            ImpactAngleVec = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = PawnOwner.TrueCenter() + ImpactAngleVec.RotatedBy(180f) * 0.5f;
            // todo: figure out what these magic numbers do
            float scale = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
            FleckMaker.Static(loc, PawnOwner.Map, AbsorbDamageFleck, scale);
            for(int i = 0; i < scale; i++)
            {
                // todo: allow users to set which fleck to throw here
                FleckMaker.ThrowDustPuff(loc, PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            }
            LastAbsorbDamageTick = Find.TickManager.TicksGame;
            KeepDisplaying();
        }
        public EffecterDef BreakEffecter => Props.breakEffecter ?? ShieldDefaults.BreakEffecter;
        public FleckDef BreakFleck => Props.breakFleck ?? ShieldDefaults.BreakFleck;
        public virtual void Break()
        {
            float scale = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, Energy);
            BreakEffecter.SpawnAttached(parent, parent.MapHeld, scale);
            // todo: another magic number
            FleckMaker.Static(PawnOwner.TrueCenter(), PawnOwner.Map, BreakFleck, 12f);
            for(int i = 0; i < 6; i++)
            {
                // todo: more magic numbers
                FleckMaker.ThrowDustPuff(PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f),
                    PawnOwner.Map, Rand.Range(0.8f, 1.2f));
            }
            Energy = 0f;
            ticksToReset = Props.startingTicksToReset;
        }
        public SoundDef ResetSound => Props.resetSound ?? ShieldDefaults.ResetSound;
        public virtual void Reset()
        {
            // todo: another location which needs null checking
            if(PawnOwner.Spawned)
            {
                ResetSound.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
                // todo: make fleck adjustable, figure out magic number
                FleckMaker.ThrowLightningGlow(PawnOwner.TrueCenter(), PawnOwner.Map, 3f);
            }
            TicksToReset = -1;
            Energy = Props.energyOnReset;
        }
        public override void CompDrawWornExtras()
        {
            base.CompDrawWornExtras();
            if (IsApparel) Draw();
        }
        public override void PostDraw()
        {
            base.PostDraw();
            if (IsBuiltIn) Draw();
        }
        protected virtual void Draw()
        {
            if(ShieldState is ShieldState.Active && ShouldDisplay)
            {
                float scale = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, Energy);
                // todo: may be null
                Vector3 drawPos = PawnOwner.Drawer.DrawPos;
                int ticksSinceLastDamage = Find.TickManager.TicksGame - LastAbsorbDamageTick;
                // magic number
                if(ticksSinceLastDamage < Props.jitterDurationTicks)
                {
                    float scaleFromTimeSinceLastDamage = (Props.jitterDurationTicks - ticksSinceLastDamage) / (float)Props.jitterDurationTicks * Props.maxDamagedJitterDist;
                    drawPos += ImpactAngleVec * scaleFromTimeSinceLastDamage;
                    scale -= scaleFromTimeSinceLastDamage;
                }
                float angle = Rand.Range(0, 360);
                Vector3 scaleVec = new Vector3(scale, 1f, scale);
                Matrix4x4 matrix = default;
                matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), scaleVec);
                Graphics.DrawMesh(MeshPool.plane10, matrix, ShieldDefaults.BubbleMat, 0);
            }
        }
        public override bool CompAllowVerbCast(Verb verb)
        {
            if (Props.blocksRangedWeapons) return !(verb is Verb_LaunchProjectile);
            if (Props.verbsToBlock.Contains(verb)) return false;
            return true;
        }
    }
}
