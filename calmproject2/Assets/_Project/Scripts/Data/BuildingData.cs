using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// ScriptableObject that defines the shared data for a building type.
    /// </summary>
    [CreateAssetMenu(menuName = "SC/Building")]
    public class BuildingData : ScriptableObject
    {
        [Tooltip("Unique identifier for this building (e.g., \"barracks\").")]
        public string id;

        [Tooltip("Prefab used to instantiate this building in the scene.")]
        public GameObject prefab;

        [Tooltip("Maximum health points of the building.")]
        public float maxHP = 1000f;
    }
}
