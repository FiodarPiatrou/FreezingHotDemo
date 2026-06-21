using Entities.Interfaces;
using Inventory;
using MVP.Presenters;
using UnityEngine;

namespace MVP.Views
{
    public class AmmoTypeView : BaseView<IEntityWeaponController>
    {
        [SerializeField] private GameObject iceObject;
        [SerializeField] private GameObject fireObject;
        void Start()
        {
            var model = GetModel();
            Presenter = new AmmoTypePresenter(this, model);
            OnModelChanged(model);
        
        }
    

        public override void OnModelChanged(IEntityWeaponController model)
        {
            iceObject.SetActive(model.CurrentAmmoType == AmmoType.Ice);
            fireObject.SetActive(model.CurrentAmmoType == AmmoType.Fire);
        }
    }
}
