using System;
using Enums;

namespace Inventory.Interfaces
{
    public interface ISphereInventory
    {
        public void AddSphere(SphereType type);
        public bool IsSphereExist(SphereType type);
        
        public event Action OnSphereAdded;
    }
}