using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime component that tracks a regular combat unit’s owner, data, and behaviour hooks.
    /// </summary>
    public class UnitController : MonoBehaviour
    {
        /// <summary>The player who owns this unit.</summary>
        public PlayerInfo Owner { get; private set; }

        /// <summary>Static data describing this unit’s base stats and modifiers.</summary>
        public UnitData Data { get; private set; }

        /// <summary>
        /// Called by SpawnSystem immediately after the unit prefab is instantiated.
        /// </summary>
        public void Init(PlayerInfo owner, UnitData data)
        {
            Owner = owner;
            Data  = data;

            // Example: apply base stats, team colours, etc.
            //   health = data.baseHP;
            //   attack = data.baseAttack;
            //   SetTeamMaterial(owner.teamColour);
        }

        // Additional behaviour (movement, combat, death) would go here.
    }
}
