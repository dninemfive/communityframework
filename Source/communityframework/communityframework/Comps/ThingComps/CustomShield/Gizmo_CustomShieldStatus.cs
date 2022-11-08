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
        public CompCustomShield Shield { get; set; }
        public Gizmo_CustomShieldStatus() { Order = -100f; }
        public override float GetWidth(float maxWidth) => Shield.Props.gizmoWidth;
        public Texture2D FullShieldBarTex
        {
            get
            {
                if (Shield.Props.barTex != null) return ContentFinder<Texture2D>.Get(Shield.Props.barTex);
                if (Shield.Props.barColor != null) return SolidColorMaterials.NewSolidColorTexture(Shield.Props.barColor.Value);
                return ShieldDefaults.FullShieldBarTex;
            }
        }
        public Texture2D EmptyShieldBarTex
        {
            get
            {
                if (Shield.Props.emptyBarTex != null) return ContentFinder<Texture2D>.Get(Shield.Props.emptyBarTex);
                if (Shield.Props.barColor != null) return SolidColorMaterials.NewSolidColorTexture(Shield.Props.emptyBarColor.Value);
                return ShieldDefaults.EmptyShieldBarTex;
            }
        }
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms _)
        {
            Rect background = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect tooltipRegion = background.ContractedBy(6f);
            Widgets.DrawWindowBackground(background);
            Rect labelRegion = tooltipRegion;
            labelRegion.height = background.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(labelRegion, Shield.Props.customNameKey?.Translate().Resolve() ?? 
                (Shield.IsApparel ? Shield.parent.LabelCap : ShieldDefaults.InbuiltShieldName));
            Rect energyBar = tooltipRegion;
            energyBar.yMin = tooltipRegion.y + tooltipRegion.height / 2f;
            float fillPercent = Shield.Energy / Mathf.Max(1f, Shield.EnergyMax);
            Widgets.FillableBar(energyBar, fillPercent, FullShieldBarTex, EmptyShieldBarTex, doBorder: false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(energyBar, $"{Shield.Energy * 100f:F0} / {Shield.EnergyMax * 100f:F0}");
            Text.Anchor = TextAnchor.UpperLeft;
            TooltipHandler.TipRegion(tooltipRegion, Shield.Props.customTooltipKey?.Translate() ?? ShieldDefaults.PersonalShieldTooltip);
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
