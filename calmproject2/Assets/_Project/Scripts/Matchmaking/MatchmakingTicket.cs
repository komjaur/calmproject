using System;

namespace SurvivalChaos
{
    /// <summary>
    /// Represents a player's entry in the matchmaking queue.
    /// </summary>
    public class MatchmakingTicket
    {
        public PlayerInfo Player { get; }
        public int Elo { get; set; }
        public DateTime QueuedAt { get; }

        private const int BaseTolerance = 50;
        private const int IncreaseIntervalSeconds = 10;
        private const int IncreaseAmount = 25;

        public MatchmakingTicket(PlayerInfo player, int elo)
        {
            Player = player;
            Elo = elo;
            QueuedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Current Elo tolerance for this ticket.
        /// </summary>
        public int CurrentTolerance
        {
            get
            {
                double seconds = (DateTime.UtcNow - QueuedAt).TotalSeconds;
                int increments = (int)(seconds / IncreaseIntervalSeconds);
                return BaseTolerance + IncreaseAmount * increments;
            }
        }
    }
}
