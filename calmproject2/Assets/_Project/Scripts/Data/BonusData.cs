using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    [CreateAssetMenu(menuName = "SC/Bonus")]
    public class BonusData : ScriptableObject
    {
        public string id;
        public List<UpgradeModifier> modifiers;
    }
}
