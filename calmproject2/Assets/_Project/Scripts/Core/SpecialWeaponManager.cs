using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public class SpecialWeaponManager : MonoBehaviour
    {
        public float cooldown = 90f;
        private readonly Dictionary<PlayerInfo, float> nextFireTime = new Dictionary<PlayerInfo, float>();

        public void FireWeapon(PlayerInfo player)
        {
            if (player == null) return;
            if (Time.time < GetNextTime(player)) return;

            nextFireTime[player] = Time.time + cooldown;
            EventBus.Raise(new GlobalWeaponFiredEvent(player));
        }

        private float GetNextTime(PlayerInfo p)
        {
            return nextFireTime.TryGetValue(p, out var t) ? t : 0f;
        }
    }
}
