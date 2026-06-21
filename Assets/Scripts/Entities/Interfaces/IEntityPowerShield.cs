using System;

namespace Entities.Interfaces
{
    public interface IEntityPowerShield
    {
        public float Charge { get; }
        public event Action OnChargeChanged;
    }
}