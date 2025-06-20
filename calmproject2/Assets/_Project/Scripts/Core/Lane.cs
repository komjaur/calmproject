using UnityEngine;

namespace SurvivalChaos
{
    public class Lane : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        public float waveInterval = 20f;
        private float timer;

        public void Setup(Transform[] points)
        {
            spawnPoints = points;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= waveInterval)
            {
                timer = 0f;
                EventBus.Raise(new WaveSpawnEvent(this));
            }
        }

        public Transform GetSpawnPoint(int playerId)
        {
            if (spawnPoints == null || playerId < 0 || playerId >= spawnPoints.Length)
                return null;
            return spawnPoints[playerId];
        }
    }
}
