using System;
using Inventory;
using UnityEngine.InputSystem;

namespace Entities.Interfaces
{
    public interface IEntityWeaponController 
    {
        public AmmoType CurrentAmmoType { get;  }
        event Action OnChangedAmmo;

    }
}
