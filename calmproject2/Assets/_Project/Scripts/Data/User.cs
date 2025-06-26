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


        // Extended gameplay analytics
        public int TotalUnitsProduced { get; private set; }
        public int TotalUnitsLost { get; private set; }
        public int TotalUnitsKilled { get; private set; }
        public float TotalDamageDealt { get; private set; }
        public float TotalDamageReceived { get; private set; }
        public int TotalGoldSpent { get; private set; }

        public float LongestMatchDuration { get; private set; }
        public float ShortestMatchDuration { get; private set; } = float.MaxValue;

        // Running totals for analytics
        public int TotalGoldAcquired { get; private set; }
        public int TotalCrystalAcquired { get; private set; }

        public int TotalMatches { get; private set; }

        public int TotalLosses => TotalMatches - TotalWins;
        public float WinRate => TotalMatches > 0 ? (float)TotalWins / TotalMatches : 0f;
        public float AverageGoldPerMatch => TotalMatches > 0 ? (float)TotalGoldAcquired / TotalMatches : 0f;

        // Track how often each race has been played and the outcome stats
        private readonly Dictionary<string, int> _racesPlayed = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _raceWins = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _raceLosses = new Dictionary<string, int>();

        public IReadOnlyDictionary<string, int> RacesPlayed => _racesPlayed;
        public IReadOnlyDictionary<string, int> RaceWins => _raceWins;
        public IReadOnlyDictionary<string, int> RaceLosses => _raceLosses;

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

            TotalUnitsProduced = 0;
            TotalUnitsLost = 0;
            TotalUnitsKilled = 0;
            TotalDamageDealt = 0f;
            TotalDamageReceived = 0f;
            TotalGoldSpent = 0;
            LongestMatchDuration = 0f;
            ShortestMatchDuration = float.MaxValue;

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

            if (durationSeconds > LongestMatchDuration)
                LongestMatchDuration = durationSeconds;
            if (durationSeconds < ShortestMatchDuration)
                ShortestMatchDuration = durationSeconds;

            if (!string.IsNullOrEmpty(race))
            {
                if (_racesPlayed.ContainsKey(race))
                    _racesPlayed[race]++;
                else
                    _racesPlayed[race] = 1;

                var targetDict = won ? _raceWins : _raceLosses;
                if (targetDict.ContainsKey(race))
                    targetDict[race]++;
                else
                    targetDict[race] = 1;
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

        /// <summary>
        /// Tracks when the player spawns a unit.
        /// </summary>
        public void RecordUnitProduced()
        {
            TotalUnitsProduced++;
        }

        /// <summary>
        /// Tracks a killed enemy unit.
        /// </summary>
        public void RecordUnitKill()
        {
            TotalUnitsKilled++;
        }

        /// <summary>
        /// Tracks loss of one of the player's units.
        /// </summary>
        public void RecordUnitLost()
        {
            TotalUnitsLost++;
        }

        /// <summary>
        /// Adds damage dealt by the player.
        /// </summary>
        public void AddDamageDealt(float amount)
        {
            if (amount > 0f)
                TotalDamageDealt += amount;
        }

        /// <summary>
        /// Adds damage received by the player.
        /// </summary>
        public void AddDamageReceived(float amount)
        {
            if (amount > 0f)
                TotalDamageReceived += amount;
        }

        /// <summary>
        /// Records gold spent on upgrades or purchases.
        /// </summary>
        public void RecordGoldSpent(int amount)
        {
            if (amount > 0)
            {
                TotalGoldSpent += amount;
                if (Gold >= amount)
                    Gold -= amount;
            }

                Crystal += amount;

        }
    }
}
