using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Gives every active player a fixed amount of gold on a repeating interval.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        [Tooltip("Seconds between each income tick.")]
        [SerializeField] private float incomeInterval = 15f;

        [Tooltip("Gold awarded to each player every tick.")]
        [SerializeField] private int goldPerTick = 25;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < incomeInterval) return;

            timer = 0f;

            foreach (var player in GameManager.Instance.ActivePlayers)
            {
                player.gold += goldPerTick;
            }

            EventBus.Raise(new IncomeTickEvent());
        }

        /// <summary>
        /// Instantly grants a lump-sum bonus to all players (handy for cheats or events).
        /// </summary>
        public void GrantBonusGold(int amount)
        {
            foreach (var player in GameManager.Instance.ActivePlayers)
            {
                player.gold += amount;
            }
        }
    }
}
