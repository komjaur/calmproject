using UnityEngine;

namespace SurvivalChaos
{
    public class UnitController : MonoBehaviour
    {
        private PlayerInfo owner;
        private UnitData data;

        public void Init(PlayerInfo owner, UnitData data)
        {
            this.owner = owner;
            this.data = data;
        }
    }
}
