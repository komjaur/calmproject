using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime component that handles health and ownership of a single building.
    /// </summary>
    public class BuildingController : MonoBehaviour
    {
        [HideInInspector]
        public PlayerInfo owner;

        [HideInInspector]
        public BuildingData data;

        [Tooltip("Current hit points. Initialized from BuildingData.maxHP.")]
        public float currentHP;

        /// <summary>
        /// Initializes the building with its owner and static data.
        /// </summary>
        public void Init(PlayerInfo owner, BuildingData data)
        {
            this.owner = owner;
            this.data  = data;
            currentHP  = data != null ? data.maxHP : 0f;
        }

        /// <summary>
        /// Applies damage and raises a BuildingDestroyedEvent when HP reaches zero.
        /// </summary>
        public void Damage(float value)
        {
            if (value <= 0f || currentHP <= 0f) return;

            currentHP -= value;

            if (currentHP <= 0f)
            {
                currentHP = 0f;
                EventBus.Raise(new BuildingDestroyedEvent(this));
            }
        }
    }
}
