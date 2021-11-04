using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// An interface that allows <c>Hediff</c>s to run a method,
    /// <c>OnHediffAdded</c>, whenever a sibling <c>Hediff</c> is added.
    /// </summary>
    public interface IHediffComp_OnHediffAdded
    {
        /// <summary>
        /// Empty method meant to be overriden. This method will run whenever
        /// a sibling <c>Hediff</c> is added to the parent <c>Hediff</c>'s
        /// <c>Pawn</c>.
        /// </summary>
        /// <param name="hediff">The <c>Hediff</c> that was just added.</param>
        void OnHediffAdded(ref Hediff hediff);
    }
}
