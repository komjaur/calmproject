using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime component attached to a hero unit to track its owner and expose hero-specific behaviour.
    /// </summary>
    public class HeroController : MonoBehaviour
    {
        /// <summary>
        /// The player who owns (summoned) this hero.
        /// </summary>
        public PlayerInfo Owner { get; private set; }

        /// <summary>
        /// Called by HeroManager right after the hero prefab is spawned.
        /// </summary>
        public void Init(PlayerInfo player)
        {
            Owner = player;
            // Add more hero-initialisation logic here (e.g., set team colours, stats, abilities).
        }
    }
}
