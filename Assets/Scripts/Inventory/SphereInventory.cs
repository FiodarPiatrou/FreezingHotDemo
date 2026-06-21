using System;
using System.Collections.Generic;
using Entities;
using Enums;
using Inventory.Interfaces;
using Managers;
using UnityEngine;

namespace Inventory
{
    public class SphereInventory : EntityComponent, ISphereInventory
    {
        private readonly HashSet<SphereType> _spheres = new();

        public void AddSphere(SphereType type)
        {
            _spheres.Add(type);
            ServiceLocator.Instance.Get<QuestManager>().AddSphere(type);
            OnSphereAdded?.Invoke();
        }

        public bool IsSphereExist(SphereType type)
        {
            return _spheres.Contains(type);
        }

        public event Action OnSphereAdded;
    }
}
