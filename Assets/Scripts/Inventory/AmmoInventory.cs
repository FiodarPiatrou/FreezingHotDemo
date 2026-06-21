using System;
using System.Collections.Generic;
using Entities;
using Inventory.Interfaces;
using UnityEngine;

namespace Inventory
{
    public class AmmoInventory : EntityComponent, IAmmoInventory
    {
        private readonly Dictionary<AmmoType, int> _ammo = new();
        
        [SerializeField] private int fireAmmoStart;
        [SerializeField] private int iceAmmoStart;

        private void Start()
        {
            AddAmmo(AmmoType.Fire, fireAmmoStart);
            AddAmmo(AmmoType.Ice, iceAmmoStart);
        }

        public int GetAmmoCount(AmmoType type)
        {
            return _ammo.GetValueOrDefault(type, 0);
        }

        public int FireAmmo => _ammo.GetValueOrDefault(AmmoType.Fire, 0);
        public int IceAmmo => _ammo.GetValueOrDefault(AmmoType.Ice, 0);

        public void AddAmmo(AmmoType type, int amount)
        {
            if (!_ammo.TryAdd(type, amount))
            {
                _ammo[type] += amount;
                OnAmmoAmountChanged?.Invoke();
            }
        }

        public bool TryUseAmmo(AmmoType type, int amount)
        {
            bool isNotEmpty = _ammo[type]!=0;
            if (isNotEmpty)
            {
                _ammo[type] -= amount;
                OnAmmoAmountChanged?.Invoke();
            }

            return isNotEmpty;
        }

        public event Action OnAmmoAmountChanged;
    }
}
