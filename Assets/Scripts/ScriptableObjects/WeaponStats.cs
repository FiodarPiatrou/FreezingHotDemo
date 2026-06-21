using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "WeaponStats", 
        menuName = "Stats/WeaponStats")
    ]
    public class WeaponStats : ScriptableObject
    {
        public float damage;
        public float range;
        public float attackSpeed;
    }
}