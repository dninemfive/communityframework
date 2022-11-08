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
    /// The <see cref="Gizmo"/> displayed next to the infopanel for the <see cref="Thing"/> the corresponding <see cref="CompCustomShield"/> is attached to.
    /// </summary>
    [StaticConstructorOnStartup]
    public class Gizmo_CustomShieldStatus : Gizmo
    {
        /// <summary>
        /// The <see cref="CompCustomShield"/> this Gizmo describes.
        /// </summary>
        public CompCustomShield Shield { get; protected set; }
        /// <summary>
        /// The label displayed on this gizmo describing the corresponding shield. If a <see cref="CompProperties_CustomShield.gizmo.customNameKey">custom name</see>
        /// is present, that is used; otherwise, if the shield is on an apparel item, it will use that item's label, otherwise falling back on the default 
        /// <see cref="ShieldDefaults.InbuiltShieldName">name for inbuilt shields</see>.
        /// </summary>
        public virtual string Label => Shield.Props.gizmo.customNameKey?.Translate().Resolve() ??
                                      (Shield.IsApparel ? Shield.parent.LabelCap : ShieldDefaults.InbuiltShieldName);
        /// <summary>
        /// The amount of energy displayed over the bar on the label. Can't be replaced in XML because i don't want to deal with old-style string interpolation,
        /// but can of course be overridden if you subclass this gizmo.
        /// </summary>
        public virtual string EnergyLabel => $"{Shield.Energy * 100f:F0} / {Shield.EnergyMax * 100f:F0}";
        /// <summary>
        /// The tooltip displayed when you hover over this gizmo. If a <see cref="CompProperties.customTooltipKey">custom tooltip</see> is present, that is used;
        /// otherwise, the default tooltip is used.
        /// </summary>
        public virtual TaggedString Tooltip => Shield.Props.gizmo.customTooltipKey?.Translate() ?? ShieldDefaults.PersonalShieldTooltip;
        #region textures
        /// <summary>
        /// The texture displayed for the portion of the energy bar which is full.
        /// </summary>
        public Texture2D FullShieldBarTex
        {
            get
            {
                if (Shield.Props.gizmo.fullBarTex != null) return ContentFinder<Texture2D>.Get(Shield.Props.gizmo.fullBarTex);
                if (Shield.Props.gizmo.fullBarColor != null) return SolidColorMaterials.NewSolidColorTexture(Shield.Props.gizmo.fullBarColor.Value);
                return ShieldDefaults.FullShieldBarTex;
            }
        }
        /// <summary>
        /// The texture displayed for the portion of the energy bar which is empty.
        /// </summary>
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
        /// <summary>
        /// The width of this gizmo, set in <see cref="Shield.Props.gizmo.width"/>.
        /// </summary>
        /// <param name="_">Unused. Interestingly, unused in vanilla also.</param>
        /// <returns>The width as described above.</returns>
        public override float GetWidth(float _) => Shield.Props.gizmo.width;
        /// <summary>
        /// Renders the gizmo.
        /// </summary>
        /// <param name="topLeft">The position of the top left corner of the gizmo.</param>
        /// <param name="maxWidth">The maximum width available for this gizmo.</param>
        /// <param name="_">Unused.</param>
        /// <returns><see cref="GizmoResult"/>(<see cref="GizmoState.Clear"/>)</returns>
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
