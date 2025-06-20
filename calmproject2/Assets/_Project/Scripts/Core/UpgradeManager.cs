using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Keeps track of upgrade levels purchased by the player / faction.
    /// </summary>
    public class UpgradeManager : MonoBehaviour
    {
        // Maps each upgrade ID to its current level.
        private readonly Dictionary<string, int> upgradeLevels = new();
        // Tracks flat stat modifiers accumulated from upgrades and bonuses.
        private readonly Dictionary<string, float> statModifiers = new();

        /// <summary>
        /// Increments the level of a given upgrade.  
        /// If the upgrade hasn’t been bought before, its level starts at 1.
        /// </summary>
        public void ApplyUpgrade(UpgradeData data)
        {
            if (data == null) return;

            if (!upgradeLevels.ContainsKey(data.id))
            {
                upgradeLevels[data.id] = 0;
            }

            upgradeLevels[data.id]++;
            if (data.modifiers != null)
            {
                foreach (var mod in data.modifiers)
                {
                    mod?.Apply(this);
                }
            }
        }

        /// <summary>
        /// Returns the current level of an upgrade ID, or 0 if never purchased.
        /// </summary>
        public int GetLevel(string id)
        {
            return upgradeLevels.TryGetValue(id, out int level) ? level : 0;
        }

        /// <summary>
        /// Adds a numeric modifier for the given stat.
        /// </summary>
        public void ApplyModifier(string stat, float value)
        {
            if (string.IsNullOrEmpty(stat))
                return;

            if (!statModifiers.ContainsKey(stat))
            {
                statModifiers[stat] = 0f;
            }

            statModifiers[stat] += value;
        }

        /// <summary>
        /// Gets the current modifier value for a stat, or 0 if none exists.
        /// </summary>
        public float GetModifier(string stat)
        {
            return statModifiers.TryGetValue(stat, out float val) ? val : 0f;
        }
    }
}
