using System;
using Entities.Interfaces;
using Enums;
using Inventory.Interfaces;
using Managers;
using UnityEngine;

namespace Entities
{
    public class InteractableSphere : EntityComponent, IInteractable
    {
        private bool _isFocused = false;

        [SerializeField] private SphereType type;
        public void Interact(IEntity entity)
        {
            entity.GetEntityComponent<ISphereInventory>().AddSphere(type);
            ServiceLocator.Instance.Get<AudioManager>().PlaySfx(SoundId.PickUpSphere);
            Destroy(gameObject);
        }

        public void SetFocused(bool value)
        {
            _isFocused = value;
            OnObjectChanged?.Invoke();
        }

        public string DisplayName => $"Take {type} Key [E]";
        public bool IsFocused => _isFocused;
        
        public event Action OnObjectChanged;
    }
}
