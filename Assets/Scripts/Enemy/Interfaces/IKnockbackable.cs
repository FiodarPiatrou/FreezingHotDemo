using UnityEngine;

namespace Enemy.Interfaces
{
    public interface IKnockbackable
    {
        void ApplyKnockback(Vector2 force, float stunDuration = 0.5f);
    }
}