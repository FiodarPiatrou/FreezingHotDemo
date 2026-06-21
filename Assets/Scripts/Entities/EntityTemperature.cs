using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Interfaces;
using Environment;
using Environment.Interfaces;
using Managers;
using Player;
using ScriptableObjects;
using Stats;
using UnityEngine;

namespace Entities
{
    public class EntityTemperature : EntityComponent, IEntityTemperature
    {
        
        private WeatherManager _weatherManager;
        
        private readonly List<ITemperatureZone> _activeZones = new();
        
        [SerializeField] private float tickRate = 0.2f;
        
        private float _currentBodyTemp;
        
        private EntityStats _stats;
        
        private IEntityHealth _health;

        private void Start()
        {
            _weatherManager = ServiceLocator.Instance.Get<WeatherManager>();
            _stats = Owner.GetEntityComponent<EntityStatsProvider>().EntityStats;
            _health = Owner.GetEntityComponent<IEntityHealth>();
            StartCoroutine(TemperatureLogicRoutine());
        }
        
        private IEnumerator TemperatureLogicRoutine()
        {
            var wait = new WaitForSeconds(tickRate);

            while (enabled)
            {
                CalculateTemperature();
   
                yield return wait;
            }
        }

        private void CalculateTemperature()
        {
            float targetTemp = _weatherManager.CurrentAmbientTemp;
            
            for (int i = _activeZones.Count - 1; i >= 0; i--)
            {
                if (_activeZones[i] != null) targetTemp += _activeZones[i].TemperatureDelta;
                else _activeZones.RemoveAt(i); 
            }
            
            float difference = Mathf.Abs(targetTemp - _currentBodyTemp);
            
            float currentSpeed = _stats.AdaptationSpeed + difference * 0.2f; 
            
            _currentBodyTemp = Mathf.MoveTowards(_currentBodyTemp, targetTemp, currentSpeed * tickRate);
    
            CheckThresholds();
            OnTemperatureChanged?.Invoke();
        }

        private void CheckThresholds()
        {
            if (_currentBodyTemp > _stats.HotTemperatureThreshold)
            {
                float diff = _currentBodyTemp - _stats.HotTemperatureThreshold;
                
                float damageToApply = diff * tickRate; 
        
                _health.TakeDamage(DamageType.Fire, damageToApply);
            }
            else if (_currentBodyTemp < _stats.ColdTemperatureThreshold)
            {
                float diff = _stats.ColdTemperatureThreshold - _currentBodyTemp;
        
                float damageToApply = diff * tickRate;
                
                _health.TakeDamage(DamageType.Ice, damageToApply); 
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<ITemperatureZone>(out var zone))
            {
                if (!_activeZones.Contains(zone))
                {
                    _activeZones.Add(zone);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<ITemperatureZone>(out var zone))
            {
                if (_activeZones.Contains(zone))
                {
                    _activeZones.Remove(zone);
                }
            }
        }
        
        public EntityStats Data => _stats;
        public float Temperature => _currentBodyTemp;
        public event Action OnTemperatureChanged;
    }
}
