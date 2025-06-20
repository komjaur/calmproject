using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// ScriptableObject describing a single research/tech upgrade.
    /// </summary>
    [CreateAssetMenu(menuName = "SC/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        [Tooltip("Unique identifier for this upgrade (e.g., \"blacksmith_attack_1\").")]
        public string id;

        [Tooltip("Gold (or generic resource) cost to purchase this upgrade.")]
        public int cost;

        [Tooltip("Stat or attribute modifiers applied each time this upgrade is researched.")]
        public List<UpgradeModifier> modifiers = new();
    }
}
