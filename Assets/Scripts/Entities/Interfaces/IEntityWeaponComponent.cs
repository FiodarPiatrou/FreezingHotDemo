using Inventory;
using UnityEngine;

namespace Entities.Interfaces
{
    public interface IEntityWeaponComponent
    {
        void Shoot(AmmoType ammoType);
    }
}