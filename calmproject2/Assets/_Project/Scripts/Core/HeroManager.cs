using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    public class HeroManager : MonoBehaviour
    {
        [SerializeField] 
        private float heroCooldown = 60f;

        private readonly Dictionary<PlayerInfo, float> nextHeroTime = new();

        /// <summary>
        /// Summons a hero for <paramref name="player"/> at <paramref name="position"/> 
        /// if their cooldown has expired.
        /// </summary>
        public void SummonHero(PlayerInfo player, GameObject prefab, Vector3 position)
        {
            if (player == null || prefab == null) return;
            if (Time.time < GetNextTime(player)) return;

            Instantiate(prefab, position, Quaternion.identity);
            nextHeroTime[player] = Time.time + heroCooldown;
        }

        /// <summary>
        /// Returns the timestamp when <paramref name="player"/> may next summon a hero.
        /// </summary>
        private float GetNextTime(PlayerInfo player)
        {
            return nextHeroTime.TryGetValue(player, out var t) ? t : 0f;
        }
    }
}
