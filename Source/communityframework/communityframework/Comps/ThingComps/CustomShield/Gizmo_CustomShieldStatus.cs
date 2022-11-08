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
    public class Gizmo_CustomShieldStatus : Gizmo
    {
        public CompCustomShield Shield { get; protected set; }
        public virtual string Label => Shield.Props.gizmo.customNameKey?.Translate().Resolve() ??
                                      (Shield.IsApparel ? Shield.parent.LabelCap : ShieldDefaults.InbuiltShieldName);
        public virtual string EnergyLabel => $"{Shield.Energy * 100f:F0} / {Shield.EnergyMax * 100f:F0}";
        public virtual TaggedString Tooltip => Shield.Props.gizmo.customTooltipKey?.Translate() ?? ShieldDefaults.PersonalShieldTooltip;
        #region textures
        public Texture2D FullShieldBarTex
        {
            get
            {
                if (Shield.Props.gizmo.fullBarTex != null) return ContentFinder<Texture2D>.Get(Shield.Props.gizmo.fullBarTex);
                if (Shield.Props.gizmo.fullBarColor != null) return SolidColorMaterials.NewSolidColorTexture(Shield.Props.gizmo.fullBarColor.Value);
                return ShieldDefaults.FullShieldBarTex;
            }
        }
        public Texture2D EmptyShieldBarTex
        {
            get
            {
                if (Shield.Props.gizmo.emptyBarTex != null) return ContentFinder<Texture2D>.Get(Shield.Props.gizmo.emptyBarTex);
                if (Shield.Props.gizmo.emptyBarColor != null) return SolidColorMaterials.NewSolidColorTexture(Shield.Props.gizmo.emptyBarColor.Value);
                return ShieldDefaults.EmptyShieldBarTex;
            }
        }
        #endregion textures
        public Gizmo_CustomShieldStatus(CompCustomShield shield)
        {
            Order = -100f;
            Shield = shield;
        }
        public override float GetWidth(float _) => Shield.Props.gizmo.width;        
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms _)
        {
            Rect background = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect tooltipRegion = background.ContractedBy(6f);
            Widgets.DrawWindowBackground(background);
            Rect labelRegion = tooltipRegion;
            labelRegion.height = background.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(labelRegion, Label);
            Rect energyBar = tooltipRegion;
            energyBar.yMin = tooltipRegion.y + tooltipRegion.height / 2f;
            float fillPercent = Shield.Energy / Mathf.Max(1f, Shield.EnergyMax);
            Widgets.FillableBar(energyBar, fillPercent, FullShieldBarTex, EmptyShieldBarTex, doBorder: false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(energyBar, EnergyLabel);
            Text.Anchor = TextAnchor.UpperLeft;
            TooltipHandler.TipRegion(tooltipRegion, Tooltip);
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
