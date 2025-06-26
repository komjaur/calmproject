using System.Collections.Generic;

namespace SurvivalChaos
{
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
