using System.Collections;
using Entities.Interfaces;
using Enums;
using Inventory;
using Managers;
using UnityEngine;

namespace Interactable
{
    public class RegenerateAmmoCrate : AmmoCrate
    { 
        [SerializeField] private float timeToRegenerate = 5f;
        
        
        private bool _isRegenerating = false;
        private float _timerCounter;
        
        public override string DisplayName
        {
            get
            {
                if (_isRegenerating)
                {
                    int minutes = Mathf.FloorToInt(_timerCounter / 60);
                    
                    int seconds = Mathf.FloorToInt(_timerCounter % 60);
                    
                    return $"Respawn in {minutes:00}:{seconds:00}";
                }
                return "Refill ammo [E]";
            }
        }

        protected override void ChangeAmmoType(WeatherState state)
        {
            //base.ChangeAmmoType(state);
        }

        public override void Interact(IEntity entity)
        {
            if (_isRegenerating) return;
            
            GiveAmmo(AmmoType.Fire, entity);
            GiveAmmo(AmmoType.Ice, entity);
            
            spriteRenderer.sprite = firstSprite;
            ServiceLocator.Instance.Get<AudioManager>().PlaySfx(SoundId.PickUpAmmo);
            
            StartCoroutine(RegenerationRoutine());
        }

        private IEnumerator RegenerationRoutine()
        {
            _isRegenerating = true;
            _timerCounter = timeToRegenerate;
            
            int lastDisplayedSecond = Mathf.CeilToInt(_timerCounter);
    
            InvokeObjectChanged();

            while (_timerCounter > 0)
            {
                yield return null;
                _timerCounter -= Time.deltaTime;
                
                int currentSecond = Mathf.CeilToInt(_timerCounter);
                if (currentSecond != lastDisplayedSecond)
                {
                    lastDisplayedSecond = currentSecond;
                    InvokeObjectChanged();
                }
            }

            _isRegenerating = false;
            _timerCounter = 0;
            spriteRenderer.sprite = secondSprite;
            InvokeObjectChanged();
        }
        
 
        private void InvokeObjectChanged()
        {
            SetFocused(IsFocused); 
        }
    }
}
