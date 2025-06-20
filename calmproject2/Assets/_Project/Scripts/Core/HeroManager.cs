using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public class HeroManager : MonoBehaviour
    {
        public float heroCooldown = 60f;
        private readonly Dictionary<PlayerInfo, float> nextHeroTime = new Dictionary<PlayerInfo, float>();

        public void SummonHero(PlayerInfo player, GameObject prefab, Vector3 position)
        {
            if (player == null || prefab == null) return;
            if (Time.time < GetNextTime(player)) return;

            Instantiate(prefab, position, Quaternion.identity);
            nextHeroTime[player] = Time.time + heroCooldown;
        }

        private float GetNextTime(PlayerInfo p)
        {
            return nextHeroTime.TryGetValue(p, out var t) ? t : 0f;
        }
    }
}
