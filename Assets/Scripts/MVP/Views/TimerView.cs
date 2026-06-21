using System;
using Managers;
using Managers.Interface;
using MVP.Presenters;
using TMPro;
using UnityEngine;

namespace MVP.Views
{
    public class TimerView : BaseView<ITimer>
    {
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private WeatherManager weatherManager;

        private void Start()
        {
            Presenter = new TimerPresenter(this, weatherManager);
        }

        public override void OnModelChanged(ITimer model)
        {
            int minutes = Mathf.FloorToInt(model.TimeRemaining / 60);
                    
            int seconds = Mathf.FloorToInt(model.TimeRemaining % 60);
                    
            timerText.text = $"New cycle in {minutes:00}:{seconds:00}";
        }
    }
}