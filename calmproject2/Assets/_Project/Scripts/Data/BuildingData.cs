using UnityEngine;

namespace SurvivalChaos
{
    [CreateAssetMenu(menuName = "SC/Building")]
    public class BuildingData : ScriptableObject
    {
        public string id;
        public GameObject prefab;
        public float maxHP = 1000f;
    }
}
