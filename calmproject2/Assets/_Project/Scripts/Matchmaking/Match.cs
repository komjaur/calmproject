using System.Collections.Generic;

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
    /// <summary>
    /// A self-contained packet that the matchmaker hands to the lobby / game-loader.
    /// Extend it with scene name, server address, etc. as needed.
    /// </summary>
    public sealed class Match
    {
        public IReadOnlyList<PlayerInfo> Players { get; }

        public Match(List<PlayerInfo> players)
        {
            Players = players;
        }
    }
}
