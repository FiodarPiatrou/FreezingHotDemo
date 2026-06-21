using Entities;
using UnityEngine;

namespace Enemy
{
    public class EnemyDeath : EntityDeath
    {
        protected override void Death()
        {
            Debug.Log("dead");
            Destroy(gameObject);
        }
    }
}