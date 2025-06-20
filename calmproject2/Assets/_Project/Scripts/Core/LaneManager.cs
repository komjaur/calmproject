using UnityEngine;

namespace SurvivalChaos
{
    public class LaneManager : MonoBehaviour
    {

        public Lane[] lanes;
        public float defaultWaveInterval = 20f;

        private void Awake()
        {
            lanes = GetComponentsInChildren<Lane>();
            foreach (var lane in lanes)
            {
                lane.waveInterval = defaultWaveInterval;
            }
        }

    }
}
