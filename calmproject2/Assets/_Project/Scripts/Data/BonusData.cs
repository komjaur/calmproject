using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Asset representing a set of upgrade modifiers that can be applied as a bonus.
    /// </summary>
    [CreateAssetMenu(menuName = "SC/Bonus")]
    public class BonusData : ScriptableObject
    {
        [Tooltip("Unique identifier for this bonus.")]
        public string id;

        [Tooltip("Stat or attribute modifications granted by this bonus.")]
        public List<UpgradeModifier> modifiers = new();

        /// <summary>
        /// Applies every modifier in this bonus to the given upgrade manager.
        /// </summary>
        public void ApplyTo(UpgradeManager manager)
        {
            if (manager == null || modifiers == null) return;

            foreach (var mod in modifiers)
            {
                mod?.Apply(manager);   // Assumes UpgradeModifier has an Apply method.
            }
        }
    }
}
