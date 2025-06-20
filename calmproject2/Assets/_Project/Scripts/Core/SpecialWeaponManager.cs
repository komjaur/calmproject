using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public class SpecialWeaponManager : MonoBehaviour
    {
        [SerializeField]
        private float cooldown = 90f;

        private readonly Dictionary<PlayerInfo, float> nextFireTime = new();

        /// <summary>
        /// Fires the player’s global special weapon if its cooldown has expired.
        /// </summary>
        public void FireWeapon(PlayerInfo player)
        {
            if (player == null) return;
            if (Time.time < GetNextTime(player)) return;

            nextFireTime[player] = Time.time + cooldown;
            EventBus.Raise(new GlobalWeaponFiredEvent(player));
        }

        /// <summary>
        /// Returns the timestamp when <paramref name="player"/> can next use their special weapon.
        /// </summary>
        private float GetNextTime(PlayerInfo player)
        {
            return nextFireTime.TryGetValue(player, out var t) ? t : 0f;
        }
    }
}
