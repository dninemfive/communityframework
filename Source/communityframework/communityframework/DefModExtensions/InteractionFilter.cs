using System.Collections.Generic;
using Verse;
using RimWorld;

namespace CF
{
    class InteractionFilter : DefModExtension
    {
        public FilterMode filterMode = FilterMode.blacklist;
        public Dictionary<InteractionDef, InteractionType> interactions =
            new Dictionary<InteractionDef, InteractionType>();

        public bool IsInteractionDisabled(InteractionDef interaction,
            bool isInitiator)
        {
            if (interactions.ContainsKey(interaction))
            {
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
            // Return false if a strict whitelist is in use, true otherwise.
            return filterMode != FilterMode.whitelist;
        }

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
