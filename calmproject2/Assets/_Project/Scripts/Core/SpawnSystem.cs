using UnityEngine;

namespace SurvivalChaos
{
    public class SpawnSystem : MonoBehaviour
    {
        private void OnEnable()  => EventBus.Subscribe<WaveSpawnEvent>(OnWaveSpawn);
        private void OnDisable() => EventBus.Unsubscribe<WaveSpawnEvent>(OnWaveSpawn);

        /// <summary>
        /// Spawns the appropriate unit for each active player when a wave is triggered.
        /// </summary>
        private void OnWaveSpawn(WaveSpawnEvent evt)
        {
            foreach (var player in GameManager.Instance.ActivePlayers)
            {
                // TODO: Retrieve the correct UnitData from the player’s race/upgrade manager.
                UnitData unitData = null;

                if (unitData?.prefab == null) continue;   // Skip if no prefab available

                Transform spawnPoint = evt.lane.GetSpawnPoint(player.id);
                if (spawnPoint == null) continue;

                var unit = Instantiate(unitData.prefab, spawnPoint.position, Quaternion.identity);

                if (unit.TryGetComponent(out UnitController controller))
                {
                    controller.Init(player, unitData);
                }
            }
        }
    }
}
