using System;
using Stats;

namespace Entities.Interfaces
{
    public interface IEntityHealth
    {
        public float MaxHealth { get; }
        public void Heal(float amount);
        
        public float CurrentHealth { get; }
        public event Action<float> OnHpChanged;
        public event Action<DamageType> OnDamageTaken;
        public event Action OnDeath;

        public void TakeDamage(DamageType type, float amount);
    }
}