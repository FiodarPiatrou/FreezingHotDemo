using System.Collections;
using Entities;
using Entities.Interfaces;
using ScriptableObjects;
using Stats;
using UnityEngine;

namespace Enemy
{
    public class EnemyAttack : EntityComponent
    {
        [SerializeField] private float attackCooldown = 1f;
        
        [Header("Hitbox")]
        [SerializeField] private GameObject attackHitboxPrefab;
        [SerializeField] private float spawnDistance = 3f;
        [SerializeField] private float windupTime = 0.5f;      // time before hitbox appears
        
        [Header("Animation / Control")]
        [SerializeField] private Animator animator;
        [SerializeField] private string attackTriggerName = "Attack";
        
        private float _lastAttackTime;
        private bool _isAttacking;

        private MonoBehaviour _movementComponent; // attempt to disable if present
        private Rigidbody2D _rb;
        private RigidbodyConstraints2D _savedConstraints;
        
        private void Awake()
        {
            var enemyStats = Owner.GetEntityComponent<EntityStatsProvider>().EntityStats as EnemyStats;

            if (enemyStats)
            {
                attackCooldown = enemyStats.attackCooldown;
                attackHitboxPrefab = enemyStats.attackHitboxPrefab ? 
                    enemyStats.attackHitboxPrefab.gameObject : attackHitboxPrefab;
                windupTime = enemyStats.windupTime;
            }
            _rb = gameObject.GetComponent<Rigidbody2D>();
            _movementComponent = Owner.GetEntityComponent<EntityMovement>() as EnemyMovement;
            animator = gameObject.GetComponent<Animator>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision detected with " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                TryAttackPlayer(collision.gameObject);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                TryAttackPlayer(collision.gameObject);
            }
        }

        private void TryAttackPlayer(GameObject player)
        {
            if (Time.time - _lastAttackTime < attackCooldown || _isAttacking)
                return;

            StartCoroutine(PerformAttack());
        }
        
        private IEnumerator PerformAttack()
        {
            _isAttacking = true;
            _lastAttackTime = Time.time;

            // stop movement: prefer disabling movement component, fallback to freezing rigidbody
            if (_movementComponent)
                _movementComponent.enabled = false;
            else if (_rb)
            {
                _savedConstraints = _rb.constraints;
                _rb.linearVelocity = Vector2.zero;
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            
            if (animator && !string.IsNullOrEmpty(attackTriggerName))
                animator.SetTrigger(attackTriggerName);

            // windup — player can see animation and try to dodge
            yield return new WaitForSeconds(windupTime);
            SpawnHitbox();
            
            // restore movement
            if (_movementComponent)
                _movementComponent.enabled = true;
            else if (_rb)
                _rb.constraints = _savedConstraints;
            
            _isAttacking = false;
        }
        // Spawn in front of the enemy. If your sprite flips by localScale.x, this will account for it.
        private void SpawnHitbox()
        {
            if (!attackHitboxPrefab)
                return;

            var facingSign = Mathf.Sign(gameObject.transform.localScale.x == 0f ? 
                1f : gameObject.transform.localScale.x);
            var spawnPos = gameObject.transform.position + 
                           (Vector3)(gameObject.transform.right * (spawnDistance * facingSign));
            var go = Instantiate(attackHitboxPrefab, spawnPos, Quaternion.identity);
            // optional: rotate or parent to owner if you want the hitbox to move with the enemy during its active time
            // go.transform.parent = Owner.transform;
            // var hitbox = go.GetComponent<AttackHitbox>();
            // if (hitbox)
            // {
            //     hitbox.SendMessage("SetDamage", SendMessageOptions.DontRequireReceiver); // if you add SetDamage method
            // }
        }
    }
}