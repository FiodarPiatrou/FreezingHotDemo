using System;
using Entities.Interfaces;
using Player;
using ScriptableObjects;
using Stats;
using UnityEngine;

namespace Entities
{
    public class EntityHealth : EntityComponent, IEntityHealth
    {
        private float _currentHealth;
        private EntityStats _stats;
        [SerializeField] private DamageType damageImmunity; 

        public float MaxHealth { get; private set; }
        public float CurrentHealth => _currentHealth;
        public event Action<float> OnHpChanged;
        public event Action<DamageType> OnDamageTaken;

        private void Start()
        {
            _stats = Owner.GetEntityComponent<EntityStatsProvider>().EntityStats;
            MaxHealth = _stats.MaxHealth;
            CurrentHp = MaxHealth;
        }
        public void Heal(float amount)
        {
            CurrentHp += amount;
        }

        private float CurrentHp
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Math.Clamp(value, 0, MaxHealth);
                OnHpChanged?.Invoke(_currentHealth);
                if (_currentHealth <= 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public void TakeDamage(DamageType type, float amount)
        {
            if (type == damageImmunity)
            {
                return;
            }
            CurrentHp -= amount * _stats.ResistMultiplayer;
            OnDamageTaken?.Invoke(type);
        }

        public event Action OnDeath;
    }
}