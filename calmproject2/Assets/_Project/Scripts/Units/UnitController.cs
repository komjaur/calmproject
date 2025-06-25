using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;                 // 2-D NavMesh

namespace SurvivalChaos
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnitController : MonoBehaviour
    {
        /* ───── Inspector ───── */

        [Header("Editor Configuration")]
        [SerializeField] private PlayerInfo _owner;
        [SerializeField] private UnitData   _data;

        [Header("Movement & AI")]
        [SerializeField] private float searchRadius = 8f;
        [SerializeField] private float moveSpeed    = 2f;

        /* ───── Public runtime properties ───── */

        public PlayerInfo Owner   { get; private set; }
        public UnitData   Data    { get; private set; }
        public float      CurrentHP { get; private set; }

        /* ───── Private state ───── */

        float            _attackTimer;
        UnitController   _target;
        NavMeshAgent     _agent;

        /* ───────────────────────────────────────────────────────────── */

        void Awake()
        {
            if (_owner != null && _data != null)
                Init(_owner, _data);

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false; // keep sprite upright
#if UNITY_2022_1_OR_NEWER
            _agent.updateUpAxis   = false; // XY plane
#endif
            _agent.speed           = moveSpeed;
            _agent.stoppingDistance = _data != null ? _data.attackRange : 0.5f;
        }

        public void Init(PlayerInfo owner, UnitData data)
        {
            Owner = owner;
            Data  = data;

            CurrentHP   = data != null ? data.baseHP : 0f;
            _attackTimer = 0f;

                var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                Shader teamShader = Shader.Find("Sprites/PlayerTeamColorSwap");
                if (teamShader != null)
                {
                    var mat = new Material(teamShader);
                    mat.SetColor("_TintColor", owner.color);
                    sr.material = mat;
                }
            }
        }

        /* ───────────────────────────────────────────────────────────── */

        void Update()
        {
            if (Data == null || CurrentHP <= 0f) return;

            // Refresh / acquire a target
            if (_target == null || _target.CurrentHP <= 0f)
                _target = FindNearestEnemy();

            if (_target == null) { _agent.ResetPath(); return; }          // Idle

            // Pursue & attack
            _agent.SetDestination(_target.transform.position);

            if (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f)
                TryAttack();
        }

        void TryAttack()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer < Data.attackCooldown) return;

            _attackTimer = 0f;
            _target.ReceiveDamage(Data.baseAttack);
        }

        UnitController FindNearestEnemy()
        {
            UnitController closest = null;
            float minDist = searchRadius;

            foreach (var unit in FindObjectsOfType<UnitController>())
            {
                if (unit == this || unit.Owner.id == Owner.id || unit.CurrentHP <= 0f)
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

        public void ReceiveDamage(float dmg)
        {
            if (dmg <= 0f || CurrentHP <= 0f) return;

            CurrentHP -= dmg;
            if (CurrentHP <= 0f)
            {
                CurrentHP = 0f;
                Destroy(gameObject);
            }
        }

        /* ───── Scene-view debug ───── */
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (_data != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _data.attackRange);
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, searchRadius);

            // ─── Simple health-bar ───
            if (Application.isPlaying && Data != null)
            {
                const float BAR_W = 1.0f, BAR_H = 0.12f, Y_OFF = 0.6f;
                float hpFrac = Mathf.Clamp01(CurrentHP / Data.baseHP);

                Vector3 barPos = transform.position + Vector3.up * Y_OFF;
                Vector3 bg = new(BAR_W, BAR_H, 0);
                Vector3 fg = new(BAR_W * hpFrac, BAR_H, 0);
                Vector3 fgOffset = Vector3.left * (BAR_W - fg.x) * 0.5f;

                Gizmos.color = Color.black;
                Gizmos.DrawCube(barPos, bg);

                Gizmos.color = Color.Lerp(Color.red, Color.green, hpFrac);
                Gizmos.DrawCube(barPos - fgOffset, fg);
            }
        }
#endif
    }
}
