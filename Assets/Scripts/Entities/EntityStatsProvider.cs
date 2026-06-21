using ScriptableObjects;
using UnityEngine;

namespace Entities
{
    public class EntityStatsProvider : EntityComponent
    {
        [SerializeField] private EntityStats entityStats;

        public EntityStats EntityStats => entityStats;
    }
}
