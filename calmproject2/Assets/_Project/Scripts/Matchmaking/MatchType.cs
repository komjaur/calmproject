using System;

namespace SurvivalChaos
{
    /// <summary>
    /// Types of matchmaking that influence how player races are selected.
    /// </summary>
    public enum MatchType
    {
        /// <summary>Each player keeps the race they queued with.</summary>
        Normal,

        /// <summary>Races are randomised for all players once a match forms.</summary>
        Chaos
    }
}
