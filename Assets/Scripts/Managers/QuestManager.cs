using Entities;
using Enums;
using Inventory.Interfaces;
using TMPro;
using UnityEngine;
using Utils;

namespace Managers
{
    public class QuestManager : BaseManager
    {
        [SerializeField] private Entity playerEntity;
        [SerializeField] private Entity rocketEntity;
        [SerializeField] private TMP_Text fireText;
        [SerializeField] private TMP_Text IceText;
        [SerializeField] private Canvas SpheresCanvas;
        [SerializeField] private Canvas InsertCanvas;
        [SerializeField] private Canvas SurviveCanvas;
    
        private bool firePickedUp;
        private bool icePickedUp;
        private bool rocketLaunched;
        private ISphereInventory _sphereInventory;
        private IRocket rocket;
        private QuestStruct _currentQuest = new QuestStruct();

        private void Start()
        {
            _sphereInventory = playerEntity.GetEntityComponent<ISphereInventory>();
            rocket = rocketEntity.GetEntityComponent<IRocket>();
            rocket.OnLaunch += Launched;
            _currentQuest.Quests = QuestsEnum.FindSpheres;
            _currentQuest.QuestState = QuestState.Current;

        }

        private void Launched()
        {
            rocketLaunched = true;
            InsertCanvas.enabled = false;
            SurviveCanvas.gameObject.SetActive(true);

        }

    

        public void AddSphere(SphereType type)
        {
        
            if (type == SphereType.Fire)
            {
                firePickedUp = true;
                fireText.fontStyle = FontStyles.Strikethrough;
            }

            if ( type == SphereType.Ice)
            {
                icePickedUp = true;
                IceText.fontStyle = FontStyles.Strikethrough;
            }

            if (icePickedUp && firePickedUp)
            {
                SpheresCanvas.enabled = false;
                InsertCanvas.gameObject.SetActive(true);
            }

        }
    }
}
