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
        }

        /// <summary>
        /// Returns the current level of an upgrade ID, or 0 if never purchased.
        /// </summary>
        public int GetLevel(string id)
        {
            return upgradeLevels.TryGetValue(id, out int level) ? level : 0;
        }
    }
}
