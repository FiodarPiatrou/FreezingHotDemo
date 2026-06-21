using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "EntityStats",
        menuName = "Stats/EntityStats"
    )]
    public class EntityStats : ScriptableObject
    {
        public float MaxHealth;
        public float Speed;
        public float AdaptationSpeed;
        public float HotTemperatureThreshold = 50;
        public float ColdTemperatureThreshold = -50;
        public float ResistMultiplayer = 0.5f;
    }
}