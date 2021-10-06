using System.Collections.Generic;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// Applied to the <c>ThingDef</c> of a race, used to determine if the race
    /// is incapable of performing certain interactions.
    /// </summary>
    class InteractionFilter : DefModExtension
    {
        /// <summary>
        /// Whether the <c>interactions</c> should be treated as a whitelist or
        /// a blacklist.
        /// </summary>
        public FilterMode filterMode = FilterMode.blacklist;

        /// <summary>
        /// A dictionary with affected <c>InteractionDef</c>s as keys, and the
        /// respective <c>InteractionType</c> as values.
        /// </summary>
        public Dictionary<InteractionDef, InteractionType> interactions =
            new Dictionary<InteractionDef, InteractionType>();

        /// <summary>
        /// Used to determine whether or not a race is capable of a given
        /// interaction.
        /// </summary>
        /// <param name="interaction">The <c>InteractionDef</c> of the
        /// interation to check.</param>
        /// <param name="isInitiator"><c>true</c> if the <c>Pawn</c> we're
        /// checking started the interaction, <c>false</c> otherwise.</param>
        /// <returns><c>true</c> if the interaction should be blocked,
        /// <c>false</c> if the interaction is allowed to occur.</returns>
        public bool IsInteractionDisabled(InteractionDef interaction,
            bool isInitiator)
        {
            if (interactions.ContainsKey(interaction))
            {
                // True if the InteractionDef and FilterMode match the modder's
                // filter.
                bool match;

                InteractionType type = interactions[interaction];
                switch (type)
                {
                    // If we're looking for interactions we initiated, return
                    // isInitiator.
                    case InteractionType.initiate:
                        match = isInitiator;
                        break;
                    // If we're looking for interactions somebody else
                    // initiated, return the inverse of isInitiator
                    case InteractionType.receive:
                        match = !isInitiator;
                        break;
                    // Always "case: both", unless more enums are added.
                    // Interaction will always match if we're not looking for a
                    // specific value of isInitiator.
                    default:
                        match = true;
                        break;
                }
                switch (filterMode)
                {
                    // If it's a blacklist, we don't want our value to match.
                    case FilterMode.blacklist:
                        return !match;
                    // If it's a whitelist, we absolutely want our value to
                    // match.
                    case FilterMode.whitelist:
                        return match;
                }
            }
            // If we've gotten to this point, the interaction is not listed.
            // Return false if a strict whitelist is in use, true otherwise.
            return filterMode != FilterMode.whitelist;
        }

        /// <summary>
        /// Used to determine whether <c>interactions</c> should behave as a
        /// blacklist or a whitelist.
        /// </summary>
        public enum FilterMode
        {
            /// <summary>
            /// An interaction will be blocked if it <b>is</b> in the list.
            /// </summary>
            blacklist,
            /// <summary>
            /// An interaction will be blocked if it <b>is not</b> in the list.
            /// </summary>
            whitelist,
        }

        /// <summary>
        /// Used to determine which "direction" an interaction should be going
        /// to be affected. For example, if "initiate" is selected, then the
        /// affected race will never initiate if blacklisted, and will never
        /// receive if whitelisted.
        /// </summary>
        public enum InteractionType
        {
            /// <summary>
            /// The interaction will be considered "in the list" if it was
            /// initiated by the target pawn.
            /// </summary>
            initiate,
            /// <summary>
            /// The interaction will be considered "in the list" if it was
            /// initiated by somebody other than the target pawn.
            /// </summary>
            receive,
            /// <summary>
            /// The interaction will always be considered as "in the list".
            /// </summary>
            both,
        }
    }
}
