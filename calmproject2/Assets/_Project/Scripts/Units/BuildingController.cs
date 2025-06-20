using UnityEngine;

namespace SurvivalChaos
{
    public class BuildingController : MonoBehaviour
    {
        public PlayerInfo owner;
        public BuildingData data;
        public float currentHP;

        public void Init(PlayerInfo owner, BuildingData data)
        {
            this.owner = owner;
            this.data = data;
            currentHP = data != null ? data.maxHP : 0f;
        }

        public void Damage(float value)
        {
            currentHP -= value;
            if (currentHP <= 0f)
            {
                EventBus.Raise(new BuildingDestroyedEvent(this));
            }
        }
    }
}
