using Entities;
using UnityEngine;

namespace Player
{
    public class PlayerDeath : EntityDeath
    {
        protected override void Death()
        {
            Destroy(gameObject);
        }
    }
}