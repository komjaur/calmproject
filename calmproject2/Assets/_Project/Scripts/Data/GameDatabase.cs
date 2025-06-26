using System;
using System.Collections.Generic;

namespace SurvivalChaos
{
    /// <summary>
    /// Simple in-memory database for storing completed match logs.
    /// </summary>
    public class GameDatabase
    {
        public static GameDatabase Instance { get; } = new GameDatabase();

        public struct MatchLog
        {
            public IReadOnlyList<PlayerInfo> Players;
            public DateTime StartedAt;
            public DateTime EndedAt;
            public float DurationSeconds;
        }

        private readonly List<MatchLog> _logs = new();
        public IReadOnlyList<MatchLog> Logs => _logs;

        public void LogMatch(Match match)
        {
            _logs.Add(new MatchLog
            {
                Players = match.Players,
                StartedAt = match.StartedAt,
                EndedAt = match.EndedAt ?? DateTime.UtcNow,
                DurationSeconds = match.DurationSeconds
            });
        }
    }
}
