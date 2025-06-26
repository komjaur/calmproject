using System;
using System.Collections.Generic;

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

        // Running totals for analytics
        public int TotalGoldAcquired { get; private set; }
        public int TotalCrystalAcquired { get; private set; }

        public int TotalMatches { get; private set; }

        // Track how often each race has been played
        private readonly Dictionary<string, int> _racesPlayed = new Dictionary<string, int>();
        public IReadOnlyDictionary<string, int> RacesPlayed => _racesPlayed;

        /// <summary>
        /// The race with the highest play count or null if none recorded.
        /// </summary>
        public string MostPlayedRace
        {
            get
            {
                string topRace = null;
                int max = 0;
                foreach (var pair in _racesPlayed)
                {
                    if (pair.Value > max)
                    {
                        topRace = pair.Key;
                        max = pair.Value;
                    }
                }
                return topRace;
            }
        }

        public User(string name)
        {
            Name = name;
            Rank = 0;
            TotalWins = 0;
            TotalPlaytime = 0f;
            AveragePlaytimePerMatch = 0f;
            Gold = 0;
            Crystal = 0;
            TotalGoldAcquired = 0;
            TotalCrystalAcquired = 0;
            TotalMatches = 0;
        }

        /// <summary>
        /// Records the result of a single match and updates aggregates.
        /// </summary>
        /// <param name="won">Whether the player won the match.</param>
        /// <param name="durationSeconds">Length of the match in seconds.</param>
        /// <param name="goldEarned">Gold earned from the match.</param>
        /// <param name="race">Name of the race played.</param>
        public void RecordMatch(bool won, float durationSeconds, int goldEarned, string race)
        {
            TotalMatches++;
            if (won)
                TotalWins++;

            TotalPlaytime += durationSeconds;
            AveragePlaytimePerMatch = TotalPlaytime / TotalMatches;
            Gold += goldEarned;
            TotalGoldAcquired += goldEarned;

            if (!string.IsNullOrEmpty(race))
            {
                if (_racesPlayed.ContainsKey(race))
                    _racesPlayed[race]++;
                else
                    _racesPlayed[race] = 1;
            }
        }

        /// <summary>
        /// Adds premium currency purchased by the player.
        /// </summary>
        public void AddCrystal(int amount)
        {
            if (amount > 0)
            {
                Crystal += amount;
                TotalCrystalAcquired += amount;
            }
        }
    }
}
