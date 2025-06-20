using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    [CreateAssetMenu(menuName = "SC/Unit")]
    public class UnitData : ScriptableObject
    {
        public string id;
        public Race race;
        public GameObject prefab;
        public float baseHP;
        public float baseAttack;
        public float goldBounty;
        public List<UpgradeModifier> modifiers; // e.g., +1 armor per level
    }
}
