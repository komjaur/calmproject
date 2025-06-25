using System.Collections.Generic;
using UnityEngine;

namespace SurvivalChaos
{
    /// <summary>
    /// ScriptableObject that defines stats and upgrade hooks for a single unit type.
    /// </summary>
    [CreateAssetMenu(menuName = "SC/Unit")]
    public class UnitData : ScriptableObject
    {
        [Tooltip("Unique identifier for this unit (e.g., \"footman\" or \"orc_grunt\").")]
        public string id;

        [Tooltip("The race / faction this unit belongs to.")]
        public Race race;

        [Tooltip("Prefab instantiated when this unit is spawned.")]
        public GameObject prefab;

        [Tooltip("Hit points before any upgrades are applied.")]
        public float baseHP;

        [Tooltip("Base attack damage before any upgrades are applied.")]
        public float baseAttack;

        [Tooltip("Seconds between consecutive attacks.")]
        public float attackCooldown = 1f;

        [Tooltip("Maximum distance to a target to perform an attack.")]
        public float attackRange = 1.5f;

        [Tooltip("Gold bounty awarded when this unit is killed.")]
        public float goldBounty;
            [Header("Movement & AI")]
    [Tooltip("How far this unit can 'see' when looking for targets.")]
    public float searchRadius = 8f;

    [Tooltip("NavMeshAgent speed for this unit.")]
    public float moveSpeed = 2f;

        [Tooltip("Prefab to fire when this unit attacks. If left null the attack is instant-melee.")]
        public Projectile projectilePrefab;   // <- new

    }
}
