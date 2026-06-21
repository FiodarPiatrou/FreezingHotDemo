using System;

namespace Inventory.Interfaces
{
    public interface IAmmoInventory
    {
        public int FireAmmo { get; }
        
        public int IceAmmo { get; }
        
        public void AddAmmo(AmmoType type, int amount);
        public bool TryUseAmmo(AmmoType type, int amount);
        
        public event Action OnAmmoAmountChanged;
    }
}