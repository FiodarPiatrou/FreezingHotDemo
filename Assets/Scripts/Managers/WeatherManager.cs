using System;
using System.Collections;
using Enums;
using Managers.Interface;
using UnityEngine;

namespace Managers
{
    public class WeatherManager : BaseManager, ITimer
    {
        public float coldTemperature = -30f; 
        public float hotTemperature = 40f;   
        public float cycleDuration = 60f;
        
        public WeatherState state = WeatherState.Cold;
    
        [SerializeField] private float currentAmbientTemp;
        public float CurrentAmbientTemp => currentAmbientTemp;
        
        public event Action<WeatherState> WeatherStateChanged;
        
        private AtmosphereManager _atmosphereManager;
        private AudioManager _audioManager;
        private float _timeRemaining;
        private void Start()
        {
            _atmosphereManager = ServiceLocator.Instance.Get<AtmosphereManager>();
            _audioManager = ServiceLocator.Instance.Get<AudioManager>();
           
            StartCoroutine(WeatherCycleRoutine());
        }

        private IEnumerator WeatherCycleRoutine()
        {
            yield return new WaitForSeconds(5f);
            
            bool isHot = state == WeatherState.Hot;
            
            if(_atmosphereManager)
            {
                _atmosphereManager.SetWeather(state);
                _audioManager.PlaySfx(SoundId.WeatherChange);
            }
            
            while (enabled)
            {
                currentAmbientTemp = isHot ? hotTemperature : coldTemperature;
                
                _timeRemaining = cycleDuration;
            
                float cycleEndTime = Time.time + cycleDuration; 

                float updateStep = 0.1f; 
                var wait = new WaitForSeconds(updateStep);
                
                while (Time.time < cycleEndTime)
                {
                    _timeRemaining = cycleEndTime - Time.time;

                    OnTimeChanged?.Invoke();
    
                    yield return wait;
                }

                _timeRemaining = 0;
                OnTimeChanged?.Invoke();
            
                isHot = !isHot;

                state = isHot ? WeatherState.Hot : WeatherState.Cold;
                
                _audioManager.PlaySfx(SoundId.WeatherChange);
                
                WeatherStateChanged?.Invoke(state);
                
                if(_atmosphereManager)
                {
                    _atmosphereManager.SetWeather(state);
                }
                
            }
        }

        public float TimeRemaining => _timeRemaining;
        public event Action OnTimeChanged;
    }
}
