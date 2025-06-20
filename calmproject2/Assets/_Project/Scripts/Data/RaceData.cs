using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// ScriptableObject that bundles all assets and settings for a race / faction.
    /// </summary>
    [CreateAssetMenu(menuName = "SC/Race")]
    public class RaceData : ScriptableObject
    {
        [Tooltip("Enum value identifying this race (e.g., Human, Orc, Undead).")]
        public Race race;

        [Tooltip("Unit types available to this race, ideally in build/tech order.")]
        public List<UnitData> units = new();

        [Tooltip("Research or tech upgrades specific to this race.")]
        public List<UpgradeData> upgrades = new();

        [Tooltip("The default hero prefab granted to the player at hero summon.")]
        public GameObject heroPrefab;

        [Tooltip("Prefab for the global special weapon (e.g., ultimate spell).")]
        public GameObject specialWeaponPrefab;
    }
}
