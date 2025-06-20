using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    [CreateAssetMenu(menuName = "SC/Race")]
    public class RaceData : ScriptableObject
    {
        public Race race;
        public List<UnitData> units;
        public List<UpgradeData> upgrades;
        public GameObject heroPrefab;
        public GameObject specialWeaponPrefab;
    }
}
