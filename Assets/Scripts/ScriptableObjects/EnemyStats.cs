using Enemy;
using Stats;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "EnemyStats",
        menuName = "Stats/EnemyStats"
    )]
    public class EnemyStats : EntityStats
    {
        public float damage = 10f;
        public AttackHitbox attackHitboxPrefab;
        public float attackCooldown = 1f;
        public float windupTime = 0.5f;
    }
}