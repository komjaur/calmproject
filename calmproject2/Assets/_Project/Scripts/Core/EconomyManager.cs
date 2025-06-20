using UnityEngine;

namespace SurvivalChaos
{
    public class EconomyManager : MonoBehaviour
    {

        public float incomeInterval = 15f;
        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= incomeInterval)
            {
                timer = 0f;
                foreach (var p in GameManager.Instance.ActivePlayers)
                {
                    p.gold += 25;
                }
                EventBus.Raise(new IncomeTickEvent());
            }
        }

        // TODO: Implement functionality

    }
}
