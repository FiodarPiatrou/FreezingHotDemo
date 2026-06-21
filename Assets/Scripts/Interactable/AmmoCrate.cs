using System;
using Entities;
using Entities.Interfaces;
using Enums;
using Inventory;
using Inventory.Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class AmmoCrate : EntityComponent, IInteractable
    {
        
        [SerializeField] protected int ammoAmount;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected Sprite firstSprite;
        [SerializeField] protected Sprite secondSprite;
        
        private WeatherManager _weatherManager;
        private AmmoType ammoType = AmmoType.Fire;
        private bool _isFocused = false;
        
        
        public event Action OnObjectChanged;

        private void Start()
        {
            _weatherManager = ServiceLocator.Instance.Get<WeatherManager>();
        

            if (_weatherManager)
            {
                _weatherManager.WeatherStateChanged += ChangeAmmoType;
                ChangeAmmoType(_weatherManager.state);
            }
        }

        private void OnDestroy()
        {
            if (_weatherManager)
            {
                _weatherManager.WeatherStateChanged -= ChangeAmmoType;
            }
        }

        protected virtual void ChangeAmmoType(WeatherState state)
        {
            ammoType = state == WeatherState.Hot ? AmmoType.Fire : AmmoType.Ice;
            
            if(ammoType == AmmoType.Fire)
                spriteRenderer.sprite = secondSprite;
            else if (ammoType == AmmoType.Ice)
                spriteRenderer.sprite = firstSprite;
            
            OnObjectChanged?.Invoke();
        }
        
        protected void GiveAmmo(AmmoType type, IEntity entity)
        {
            var inv = entity.GetEntityComponent<IAmmoInventory>();
            inv?.AddAmmo(type, ammoAmount);
        }

        public virtual void Interact(IEntity entity)
        {
            GiveAmmo(ammoType, entity);
            ServiceLocator.Instance.Get<AudioManager>().PlaySfx(SoundId.PickUpAmmo);
            Destroy(gameObject); 
        }

        public void SetFocused(bool value)
        {
            _isFocused = value;
            OnObjectChanged?.Invoke();
        }

  
        public virtual string DisplayName => $"Take {ammoType} Ammo [E]";
    
        public bool IsFocused => _isFocused;
    }
}
