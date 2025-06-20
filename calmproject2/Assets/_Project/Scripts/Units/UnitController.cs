using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime component that tracks a regular combat unit’s owner, data, and behaviour hooks.
    /// </summary>
    public class UnitController : MonoBehaviour
    {
        /// <summary>The player who owns this unit.</summary>
        public PlayerInfo Owner { get; private set; }

        /// <summary>Static data describing this unit’s base stats and modifiers.</summary>
        public UnitData Data { get; private set; }

        /// <summary>Current hit points for this unit.</summary>
        public float CurrentHP { get; private set; }

        private float _attackTimer;

        /// <summary>
        /// Called by SpawnSystem immediately after the unit prefab is instantiated.
        /// </summary>
        public void Init(PlayerInfo owner, UnitData data)
        {
            Owner = owner;
            Data  = data;

            CurrentHP = data != null ? data.baseHP : 0f;
            _attackTimer = 0f;

            // Example: apply base stats, team colours, etc.
            //   health = data.baseHP;
            //   attack = data.baseAttack;
            //   SetTeamMaterial(owner.teamColour);
        }

        private void Update()
        {
            if (Data == null || CurrentHP <= 0f) return;

            _attackTimer += Time.deltaTime;

            if (_attackTimer < Data.attackCooldown) return;

            var target = FindNearestEnemy();
            if (target == null) return;

            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist > Data.attackRange) return;

            _attackTimer = 0f;
            target.ReceiveDamage(Data.baseAttack);
        }

        private UnitController FindNearestEnemy()
        {
            UnitController closest = null;
            float minDist = float.MaxValue;

            foreach (var unit in FindObjectsOfType<UnitController>())
            {
                if (unit == this || unit.Owner == Owner || unit.CurrentHP <= 0f)
                    continue;

                float d = Vector3.Distance(transform.position, unit.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    closest = unit;
                }
            }

            return closest;
        }

        /// <summary>Applies damage to this unit and destroys it if health reaches zero.</summary>
        public void ReceiveDamage(float value)
        {
            if (value <= 0f || CurrentHP <= 0f) return;

            CurrentHP -= value;
            if (CurrentHP <= 0f)
            {
                CurrentHP = 0f;
                Destroy(gameObject);
            }
        }

        // Additional behaviour (movement, death rewards, etc.) would go here.
    }
}
