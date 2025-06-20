
using System.Collections.Generic;

using UnityEngine;

namespace SurvivalChaos
{

    [CreateAssetMenu(menuName = "SC/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public string id;
        public int cost;
        public List<UpgradeModifier> modifiers;

    public class UpgradeData : MonoBehaviour
    {

    }
}
