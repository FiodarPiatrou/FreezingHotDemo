using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "SoundData",
        menuName = "Audio/SoundData"
    )]
    public class SoundData : ScriptableObject
    {
        public SoundId id;
        [Header("Clips")]
        public AudioClip[] clips;

        [Header("Volume")]
        [Range(0f, 1f)] public float volume = 1f;

        [Header("Pitch")]
        public float minPitch = 0.95f;
        public float maxPitch = 1.05f;

        public AudioClip GetRandomClip()
        {
            if (clips == null || clips.Length == 0)
                return null;

            return clips[Random.Range(0, clips.Length)];
        }

        public float GetRandomPitch()
        {
            return Random.Range(minPitch, maxPitch);
        }
        
        public bool loop;
    }
}