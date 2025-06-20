using UnityEngine;

namespace SurvivalChaos
{
    public class SpawnSystem : MonoBehaviour
    {

        private void OnEnable()
        {
            EventBus.Subscribe<WaveSpawnEvent>(OnWaveSpawn);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<WaveSpawnEvent>(OnWaveSpawn);
        }

        private void OnWaveSpawn(WaveSpawnEvent evt)
        {
            foreach (var player in GameManager.Instance.ActivePlayers)
            {
                UnitData ud = null; // Acquire from player race/upgrade manager.
                // Placeholder: no spawn if not assigned
                if (ud == null || ud.prefab == null) continue;

                Transform sp = evt.lane.GetSpawnPoint(player.id);
                if (sp != null)
                {
                    var unit = Instantiate(ud.prefab, sp.position, Quaternion.identity);
                    var ctrl = unit.GetComponent<UnitController>();
                    if (ctrl != null)
                        ctrl.Init(player, ud);
                }
            }
        }

    }
}
