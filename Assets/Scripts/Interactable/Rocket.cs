using System;
using System.Linq;
using Entities.Interfaces;
using Enums;
using Inventory;
using Managers;
using UnityEngine;

namespace Entities
{
    public class Rocket : EntityComponent, IInteractable, IRocket
    {
        
        private bool _isFocused = false;
        private bool _isLaunched = false;
        
        public void Interact(IEntity entity)
        {
            var sphereInventory = entity.GetEntityComponent<SphereInventory>();
            
            if (sphereInventory == null)
                return;
            
            if (HasAllSpheres(sphereInventory) && !_isLaunched)
            {
                LaunchRocket();
            }
            else
            {
                ShowMissingSpheres(sphereInventory);
            }
        }
        
        private bool HasAllSpheres(SphereInventory inventory)
        {
            // Get all sphere types defined in the SphereType enum
            var allSphereTypes = Enum.GetValues(typeof(SphereType)).Cast<SphereType>();

            // Check if inventory contains all sphere types
            return allSphereTypes.All(inventory.IsSphereExist);
        }
        
        private void LaunchRocket()
        {
            OnLaunch?.Invoke();
            Debug.Log("Rocket Launched!");
        }
        
        private void ShowMissingSpheres(SphereInventory inventory)
        {
            var allSphereTypes = Enum.GetValues(typeof(SphereType)).Cast<SphereType>();
            var missingSpheres = allSphereTypes.Where(type => !inventory.IsSphereExist(type));
            
            Debug.Log("Missing Spheres: " + string.Join(", ", missingSpheres));
        }
        
        public void SetFocused(bool value)
        {
            _isFocused = value;
            OnObjectChanged?.Invoke();
            
        }

        public virtual string DisplayName => "Launch Rocket [E]";
        public bool IsFocused => _isFocused;
        public event Action OnObjectChanged;
        public event Action OnLaunch;
    }
}