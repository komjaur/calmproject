using System;
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

        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }

        public float DurationSeconds
            => (float)((EndedAt ?? DateTime.UtcNow) - StartedAt).TotalSeconds;

        public Match(List<PlayerInfo> players)
        {
            Players = players;
        }

        /// <summary>Marks the match as started.</summary>
        public void Start()
        {
            StartedAt = DateTime.UtcNow;
            EndedAt = null;
        }

        /// <summary>Marks the match as finished.</summary>
        public void Finish()
        {
            if (EndedAt == null)
                EndedAt = DateTime.UtcNow;
        }
    }
}
