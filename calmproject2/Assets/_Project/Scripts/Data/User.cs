using System;

namespace SurvivalChaos
{
    /// <summary>
    /// Persistent profile data for a player.
    /// </summary>
    [Serializable]
    public class User
    {
        public string Name;
        public int Rank;
        public int TotalWins;
        public float TotalPlaytime;             // Seconds
        public float AveragePlaytimePerMatch;   // Seconds
        public int Gold;        // Earned through gameplay
        public int Crystal;     // Premium currency purchased with real money

        public int TotalMatches { get; private set; }

        public User(string name)
        {
            Name = name;
            Rank = 0;
            TotalWins = 0;
            TotalPlaytime = 0f;
            AveragePlaytimePerMatch = 0f;
            Gold = 0;
            Crystal = 0;
            TotalMatches = 0;
        }

        /// <summary>
        /// Records the result of a single match and updates aggregates.
        /// </summary>
        /// <param name="won">Whether the player won the match.</param>
        /// <param name="durationSeconds">Length of the match in seconds.</param>
        /// <param name="goldEarned">Gold earned from the match.</param>
        public void RecordMatch(bool won, float durationSeconds, int goldEarned)
        {
            TotalMatches++;
            if (won)
                TotalWins++;

            TotalPlaytime += durationSeconds;
            AveragePlaytimePerMatch = TotalPlaytime / TotalMatches;
            Gold += goldEarned;
        }

        /// <summary>
        /// Adds premium currency purchased by the player.
        /// </summary>
        public void AddCrystal(int amount)
        {
            if (amount > 0)
                Crystal += amount;
        }
    }
}
