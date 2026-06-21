using Inventory.Interfaces;
using MVP.Presenters;
using TMPro;
using UnityEngine;

namespace MVP.Views
{
    public class AmmoAmountView : BaseView<IAmmoInventory>
    {
        [SerializeField] private TMP_Text fireAmmoLabel;
        [SerializeField] private TMP_Text iceAmmoLabel;
        
        private void Start()
        {
            var model = GetModel();
            Presenter = new AmmoAmountPresenter(this, model);
            OnModelChanged(model);
        }
        public override void OnModelChanged(IAmmoInventory model)
        {
            fireAmmoLabel.text = model.FireAmmo.ToString();
            iceAmmoLabel.text = model.IceAmmo.ToString();
        }
    }
}