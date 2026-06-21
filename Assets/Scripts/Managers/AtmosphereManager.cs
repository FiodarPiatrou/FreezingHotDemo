using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Managers
{
    public class AtmosphereManager : BaseManager
    {
        [Serializable]
        private struct WeatherProfileEntry
        {
            public WeatherState state;
            public VolumeProfile profile;
            public GameObject effectGroup;
        }

        [SerializeField] private float flashDuration = 0.5f;
        
        [SerializeField] private VolumeProfile flashProfile; 
        
        [SerializeField] private List<WeatherProfileEntry> weatherProfilesData;

        private CameraShakeManager _cameraShakeManager;
        private Volume _volumeA;
        private Volume _volumeB;
        private Volume _flashVolume; 

        private bool _isUsingVolumeA = true;

        private Dictionary<WeatherState, WeatherProfileEntry> _profilesMap;
        private Coroutine _transitionCoroutine;
        private GameObject _currentEffectGroup;

        private void Awake()
        {
            InitializeDictionary();
            SetupVolumes();
            
            foreach (var entry in weatherProfilesData)
            {
                if (entry.effectGroup) entry.effectGroup.SetActive(false);
            }
        }

        private void Start()
        {
            _cameraShakeManager = ServiceLocator.Instance.Get<CameraShakeManager>();
        }

        private void InitializeDictionary()
        {
            _profilesMap = new Dictionary<WeatherState, WeatherProfileEntry>();
            foreach (var entry in weatherProfilesData)
            {
                _profilesMap.TryAdd(entry.state, entry);
            }
        }

        private void SetupVolumes()
        {
         
            GameObject objA = new GameObject("WeatherVolume_A");
            objA.transform.SetParent(transform);
            _volumeA = objA.AddComponent<Volume>();
            _volumeA.weight = 1f;
            _volumeA.priority = 1; 
            
            GameObject objB = new GameObject("WeatherVolume_B");
            objB.transform.SetParent(transform);
            _volumeB = objB.AddComponent<Volume>();
            _volumeB.weight = 0f;
            _volumeB.priority = 1;

            GameObject objFlash = new GameObject("WeatherVolume_Flash");
            objFlash.transform.SetParent(transform);
            _flashVolume = objFlash.AddComponent<Volume>();
            _flashVolume.isGlobal = true;

 
            _flashVolume.profile = flashProfile;
            _flashVolume.weight = 0f;


            _flashVolume.priority = 10;
        }

        public void SetWeather(WeatherState newState)
        {
            if (!_profilesMap.TryGetValue(newState, out var targetEntry)) return;

            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = StartCoroutine(ShockTransitionRoutine(targetEntry));
        }

        private IEnumerator ShockTransitionRoutine(WeatherProfileEntry targetEntry)
        {
            Volume activeVolume = _isUsingVolumeA ? _volumeA : _volumeB;
            Volume futureVolume = _isUsingVolumeA ? _volumeB : _volumeA;

            futureVolume.profile = targetEntry.profile;
            futureVolume.weight = 0f;

            if (_cameraShakeManager)
            {
                _cameraShakeManager.Shake(flashDuration + 2f, 0.5f); 
            }
            
        
            float halfDuration = flashDuration / 2f;
            float timer = 0f;


            bool hasFlash = _flashVolume.profile;

            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = timer / halfDuration;

                if (hasFlash) _flashVolume.weight = t; 

                yield return null;
            }

            if (hasFlash) _flashVolume.weight = 1f;

            if (_currentEffectGroup)
                _currentEffectGroup.SetActive(false);
            
            if (targetEntry.effectGroup)
                targetEntry.effectGroup.SetActive(true);

            activeVolume.weight = 0f;
            futureVolume.weight = 1f;

            _currentEffectGroup = targetEntry.effectGroup;
            _isUsingVolumeA = !_isUsingVolumeA;

            yield return null; 


            timer = 0f;
            while (timer < halfDuration)
            {
                timer += Time.deltaTime;
                float t = timer / halfDuration;

                if (hasFlash) _flashVolume.weight = 1f - t; 

                yield return null;
            }

            if (hasFlash) _flashVolume.weight = 0f;
        }
    }
}
