using Enums;
using Inventory.Interfaces;
using MVP.Presenters;
using UnityEngine;

namespace MVP.Views
{
    public class SphereView : BaseView<ISphereInventory>
    {
      
        [SerializeField] private SphereType type;
        [SerializeField] private GameObject spherePrefab;
        private void Start()
        {
            var model = GetModel();
            Presenter = new SpherePresenter(this, model);
            OnModelChanged(model);
        }
        
        public override void OnModelChanged(ISphereInventory model)
        {
            spherePrefab.SetActive(model.IsSphereExist(type));
        }
    }
}
