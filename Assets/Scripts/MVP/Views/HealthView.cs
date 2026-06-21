using System.Globalization;
using Entities.Interfaces;
using MVP.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class HealthView : BaseView<IEntityHealth>
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text currentHealthText;
        private void Start()
        {
            var health = GetModel();
            Presenter = new HealthPresenter(this, health);
            currentHealthText.text = health.CurrentHealth.ToString(CultureInfo.InvariantCulture);
        }
        
        public override void OnModelChanged(IEntityHealth model)
        {
            currentHealthText.text = model.CurrentHealth.ToString("F1", CultureInfo.InvariantCulture);
            healthBar.fillAmount = model.CurrentHealth / model.MaxHealth;
        }
    }
}
