using UnityEngine;

namespace SurvivalChaos
{
    public class HeroController : MonoBehaviour
    {
        private PlayerInfo owner;

        public void Init(PlayerInfo player)
        {
            owner = player;
        }
    }
}
