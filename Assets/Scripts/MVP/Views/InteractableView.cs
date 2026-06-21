using Entities.Interfaces;
using MVP.Presenters;
using TMPro;
using UnityEngine;

namespace MVP.Views
{
    public class InteractableView : BaseView<IInteractable>
    {
        [SerializeField] private TMP_Text interactableLabel;
        private void Start()
        {
            var model = GetModel();
            Presenter = new InteractablePresenter(this, model);
            OnModelChanged(model);
        }
        public override void OnModelChanged(IInteractable model)
        {
            Debug.Log(model.DisplayName);
            Debug.Log(model.IsFocused);
            interactableLabel.text = model.DisplayName;
            interactableLabel.enabled = model.IsFocused;
        }
    }
}