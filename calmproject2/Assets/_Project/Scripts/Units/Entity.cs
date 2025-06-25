using UnityEngine;
using UnityEngine.AI;   // 2-D NavMesh

namespace SurvivalChaos
{
    /// <summary>
    /// Runtime behaviour for every spawned Unit (melee or ranged).
    /// All tunable stats now live in <see cref="UnitData"/>.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Entity : MonoBehaviour
    {
        /* ───── Inspector wiring ───── */

        [Header("Editor Configuration")]
        [SerializeField] private PlayerInfo _owner;   // set by spawner
        [SerializeField] private UnitData   _data;    // set by spawner

        /* ───── Public runtime props ───── */

        public PlayerInfo Owner   { get; private set; }
        public UnitData   Data    { get; private set; }
        public float      CurrentHP { get; private set; }

        /* ───── Private state ───── */

        float        _attackTimer;
        Entity       _target;
        NavMeshAgent _agent;

        /* ───────────────────────────────────────────────────────────── */

        void Awake()
        {
            // Allow drag-&-dropping a prefab straight into the scene
            if (_owner != null && _data != null)
                Init(_owner, _data);

            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false; // keep 2-D sprite upright

#if UNITY_2022_1_OR_NEWER
            _agent.updateUpAxis = false;   // work in XY plane
#endif

            if (_data != null)
            {
                _agent.speed            = _data.moveSpeed;
                _agent.stoppingDistance = _data.attackRange;
            }
        }

        /// <summary>Called by the spawner right after instantiation.</summary>
        public void Init(PlayerInfo owner, UnitData data)
        {
            Owner = owner;
            Data  = data;

            CurrentHP    = data != null ? data.baseHP : 0f;
            _attackTimer = 0f;

            // Apply team-tint material (optional)
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

            // (Re)acquire target
            if (_target == null || _target.CurrentHP <= 0f)
                _target = FindNearestEnemy();

            if (_target == null)
            {
                _agent.ResetPath();   // Nothing to do — idle
                return;
            }

            // Pursue target
            _agent.SetDestination(_target.transform.position);

            // Attack when close enough
            if (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f)
                TryAttack();
        }

        /* ─── Combat ─── */

        void TryAttack()
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer < Data.attackCooldown) return;

            _attackTimer = 0f;

            // Ranged?
            if (Data.projectilePrefab != null)
            {
                Projectile p = Instantiate(Data.projectilePrefab, transform.position, Quaternion.identity);
                p.damage = Data.baseAttack;
                p.target = _target;
            }
            else // Melee / instant hit
            {
                _target.ReceiveDamage(Data.baseAttack);
            }
        }

        Entity FindNearestEnemy()
        {
            Entity closest = null;
            float  minDist = Data.searchRadius;

            foreach (var unit in FindObjectsOfType<Entity>())
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

        /* ───── Scene-view debug helpers ───── */
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            // Attack range (red) and search radius (cyan)
            if (_data != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _data.attackRange);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, _data.searchRadius);
            }

            // Health bar (play mode only)
            if (Application.isPlaying && Data != null)
            {
                const float BAR_W = 1.0f, BAR_H = 0.12f, Y_OFF = 0.6f;
                float hpFrac = Mathf.Clamp01(CurrentHP / Data.baseHP);

                Vector3 barPos    = transform.position + Vector3.up * Y_OFF;
                Vector3 bgSize    = new(BAR_W, BAR_H, 0);
                Vector3 fgSize    = new(BAR_W * hpFrac, BAR_H, 0);
                Vector3 fgOffset  = Vector3.left * (BAR_W - fgSize.x) * 0.5f;

                Gizmos.color = Color.black;
                Gizmos.DrawCube(barPos, bgSize);

                Gizmos.color = Color.Lerp(Color.red, Color.green, hpFrac);
                Gizmos.DrawCube(barPos - fgOffset, fgSize);
            }
        }
#endif
    }
}
