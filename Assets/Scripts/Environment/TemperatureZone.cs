using Entities;
using Environment.Interfaces;
using UnityEngine;

namespace Environment
{
    public class TemperatureZone : EntityComponent, ITemperatureZone
    {
        public float temperatureDelta = 50f;
        public float TemperatureDelta => temperatureDelta;
    }
}