using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime component that tracks a regular combat unit’s owner, data, and behaviour hooks.
    /// </summary>
    public class UnitController : MonoBehaviour
    {
        [Header("Editor Configuration")]

        [Tooltip("Assign this unit's owner in the editor for testing.")]
        [SerializeField] private PlayerInfo _owner;

        [Tooltip("Assign this unit's base data (HP, attack, cooldown, etc.).")]
        [SerializeField] private UnitData _data;

        /// <summary>The player who owns this unit.</summary>
        public PlayerInfo Owner { get; private set; }

        /// <summary>Static data describing this unit’s base stats and modifiers.</summary>
        public UnitData Data { get; private set; }

        /// <summary>Current hit points for this unit.</summary>
        public float CurrentHP { get; private set; }

        private float _attackTimer;

        private void Awake()
        {
            if (_owner != null && _data != null)
                Init(_owner, _data);
        }

        public void Init(PlayerInfo owner, UnitData data)
        {
            Owner = owner;
            Data = data;

            CurrentHP = data != null ? data.baseHP : 0f;
            _attackTimer = 0f;

            // Apply the owner's team colour if a SpriteRenderer is present
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
                sr.color = owner.color;
        }

        private void Update()
        {
            if (Data == null || CurrentHP <= 0f) return;

            _attackTimer += Time.deltaTime;
            if (_attackTimer < Data.attackCooldown) return;

            var target = FindNearestEnemy();
            if (target == null) return;

            float dist = Vector2.Distance(transform.position, target.transform.position);
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

                float d = Vector2.Distance(transform.position, unit.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    closest = unit;
                }
            }

            return closest;
        }

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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_data != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _data.attackRange);
            }
        }
#endif
    }
}
