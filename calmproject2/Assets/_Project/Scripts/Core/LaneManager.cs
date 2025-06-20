using UnityEngine;

namespace SurvivalChaos
{
    public class LaneManager : MonoBehaviour
    {
        [SerializeField]
        private float defaultWaveInterval = 20f;

        private Lane[] lanes;

        private void Awake()
        {
            lanes = GetComponentsInChildren<Lane>();

            foreach (var lane in lanes)
            {
                lane.waveInterval = defaultWaveInterval;
            }
        }

        /// <summary>
        /// Updates the wave-spawn interval for every lane at runtime.
        /// </summary>
        public void SetWaveInterval(float seconds)
        {
            defaultWaveInterval = seconds;

            foreach (var lane in lanes)
            {
                lane.waveInterval = seconds;
            }
        }
    }
}
