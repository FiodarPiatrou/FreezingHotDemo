using System.Globalization;
using Entities.Interfaces;
using MVP.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class ShieldChargeView : BaseView<IEntityPowerShield>
    {
        [SerializeField] private Image chargeBar;
        [SerializeField] private TMP_Text chargeText;
        private void Start()
        {
            var shield = GetModel();
            Presenter = new PowerShieldPresenter(this, shield);
        }
        
        public override void OnModelChanged(IEntityPowerShield model)
        {
            chargeBar.fillAmount = model.Charge;

            chargeText.text = Mathf.Approximately(chargeBar.fillAmount, 1) ? "Activate [Space]" : "Charging...";
        }
    }
}