using Entities.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class InteractablePresenter : BasePresenter<IInteractable>
    {
        public InteractablePresenter(BaseView<IInteractable> view, IInteractable model) : base(view, model)
        {
            Model.OnObjectChanged += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnObjectChanged -= OnModelChanged;
        }
    }
}