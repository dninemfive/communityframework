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
        public CompCustomShield Shield { get; private set; }
        public Gizmo_CustomShieldStatus() { Order = -100f; }
        public override float GetWidth(float maxWidth) => Shield.Props.gizmoWidth;
        public Texture2D FullShieldBarTex => ShieldDefaults.FullShieldBarTex;
        public Texture2D EmptyShieldBarTex => ShieldDefaults.EmptyShieldBarTex;
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            Rect background = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
            Rect foreground = background.ContractedBy(6f);
            Widgets.DrawWindowBackground(background);
            Rect foreground2 = foreground;
            foreground2.height = background.height / 2f;
            Text.Font = GameFont.Tiny;
            Widgets.Label(foreground2, Shield.IsApparel ? Shield.parent.LabelCap : "ShieldInbuilt".Translate().Resolve());
            Rect energyBar = foreground;
            energyBar.yMin = foreground.y + foreground.height / 2f;
            float fillPercent = Shield.Energy / Mathf.Max(1f, Shield.EnergyMax);
            Widgets.FillableBar(energyBar, fillPercent, FullShieldBarTex, EmptyShieldBarTex, doBorder: false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(energyBar, $"{Shield.Energy * 100f:F0} / {Shield.EnergyMax * 100f:F0}");
            Text.Anchor = TextAnchor.UpperLeft;
            TooltipHandler.TipRegion(foreground, "ShieldPersonalTip".Translate());
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
