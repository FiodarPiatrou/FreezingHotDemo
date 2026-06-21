using Entities.Interfaces;
using Stats;
using UnityEngine;

namespace Enemy
{
    public class AttackHitbox : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private DamageType damageType = DamageType.Common;
        [SerializeField] private float lifetime = 0.1f;
        [SerializeField] private bool destroyOnHit = true;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            var health = other.GetComponent<IEntityHealth>();
            if (health != null)
            {
                health.TakeDamage(damageType, damage);
                if (destroyOnHit)
                    Destroy(gameObject);
            }
        }
    }
}