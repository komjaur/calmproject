
using System.Collections.Generic;
=======

using UnityEngine;

namespace SurvivalChaos
{
    public class UpgradeManager : MonoBehaviour
    {

        private readonly Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

        public void ApplyUpgrade(UpgradeData data)
        {
            if (data == null) return;
            if (!upgradeLevels.ContainsKey(data.id)) upgradeLevels[data.id] = 0;
            upgradeLevels[data.id]++;
        }

        public int GetLevel(string id)
        {
            return upgradeLevels.TryGetValue(id, out var lv) ? lv : 0;
        }

    }
}
