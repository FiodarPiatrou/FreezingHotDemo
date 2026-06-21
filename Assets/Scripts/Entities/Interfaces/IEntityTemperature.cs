using System;
using ScriptableObjects;

namespace Entities.Interfaces
{
    public interface IEntityTemperature
    {
        public EntityStats Data { get; }
        
        public float Temperature { get; }
        public event Action OnTemperatureChanged;
    }
}