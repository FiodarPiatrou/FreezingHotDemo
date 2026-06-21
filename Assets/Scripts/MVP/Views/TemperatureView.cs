using System.Globalization;
using Entities;
using Entities.Interfaces;
using Managers;
using MVP.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class TemperatureView : BaseView<IEntityTemperature>
    {
        [SerializeField] private TMP_Text temperatureText;
        [SerializeField] private Slider temperatureSlider;
        
        private WeatherManager _weatherManager;
        private void Start()
        {
            _weatherManager = ServiceLocator.Instance.Get<WeatherManager>();
            
            if (temperatureSlider)
            {
                temperatureSlider.minValue = _weatherManager.coldTemperature;
                temperatureSlider.maxValue = _weatherManager.hotTemperature;
                temperatureSlider.interactable = false;
            }
          
            var model = GetModel();
       
            Presenter = new TemperaturePresenter(this, model);
        }
        
        
        public override void OnModelChanged(IEntityTemperature model)
        {
            if (temperatureText)
            {
                var temp = model.Temperature.ToString("F1", CultureInfo.InvariantCulture)+ " °C";
                if(model.Temperature >= _weatherManager.hotTemperature)
                    temperatureText.text = $"Above +{_weatherManager.hotTemperature} °C!";
                else if(model.Temperature <= _weatherManager.coldTemperature)
                    temperatureText.text = $"Below {_weatherManager.coldTemperature} °C!";
                else
                    temperatureText.text = model.Temperature > 0 ? "+"+temp : temp;
            }

            if (temperatureSlider)
                temperatureSlider.value = model.Temperature;
        }
    }
}