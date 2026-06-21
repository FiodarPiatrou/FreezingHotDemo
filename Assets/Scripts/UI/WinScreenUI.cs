using Entities;
using UnityEngine;
using Weapon;

namespace UI
{
    public class WinScreenUI : DeathScreenUI
    {
        
        [SerializeField] private Entity playerEntity;
        private void Start()
        {
            if (entity)
            {
                var health = entity.GetEntityComponent<IRocket>();
                if (health != null)
                {
                    health.OnLaunch += ShowScreen;
                }
                
            }
        }
        
        protected override void ShowScreen()
        {
            Destroy(playerEntity);
            base.ShowScreen();
        }
    }
}
