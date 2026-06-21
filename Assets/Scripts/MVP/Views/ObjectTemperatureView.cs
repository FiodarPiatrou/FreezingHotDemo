using System;
using System.Globalization;
using Environment.Interfaces;
using MVP.Presenters;
using TMPro;
using UnityEngine;

namespace MVP.Views
{
    public class ObjectTemperatureView : BaseView<ITemperatureZone>
    {
        [SerializeField] private TMP_Text interactableLabel;

        private void Start()
        {
            var model = GetModel();
            Presenter = new ObjectTemperaturePresenter(this, model);
            var temp = model.TemperatureDelta.ToString("F1", CultureInfo.InvariantCulture)+ " °C";
            interactableLabel.text = model.TemperatureDelta > 0 ? "+"+temp : temp;
        }

        public override void OnModelChanged(ITemperatureZone model)
        {
            
        }
    }
}
