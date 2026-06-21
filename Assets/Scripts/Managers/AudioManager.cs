using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class AudioManager : BaseManager
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;

        [Header("Sounds")]
        [SerializeField] private SoundData[] sounds;

        private Dictionary<SoundId, SoundData> _soundMap;

        private void Awake()
        {
            _soundMap = new Dictionary<SoundId, SoundData>();
            foreach (var sound in sounds)
            {
                _soundMap[sound.id] = sound;
            }
        }

        public void PlaySfx(SoundId sound)
        {
            if (!_soundMap.TryGetValue(sound, out var soundData))
                return;
            
            var clip = soundData.GetRandomClip();
            if (!clip)
                return;
            sfxSource.pitch = soundData.GetRandomPitch();
            sfxSource.PlayOneShot(clip, soundData.volume);
        }

        public void PlayUi(SoundId id)
        {
            if (!_soundMap.TryGetValue(id, out var sound))
                return;

            var clip = sound.GetRandomClip();
            if (!clip)
                return;

            uiSource.pitch = sound.GetRandomPitch();
            uiSource.PlayOneShot(clip, sound.volume);
        }

        public void PlayMusic(SoundId id)
        {
            if (!_soundMap.TryGetValue(id, out var sound))
                return;

            var clip = sound.GetRandomClip();
            if (!clip)
                return;

            musicSource.clip = clip;
            musicSource.loop = sound.loop;
            musicSource.volume = sound.volume;
            musicSource.pitch = sound.GetRandomPitch();
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}