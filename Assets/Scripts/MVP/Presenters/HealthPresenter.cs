using Entities.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class HealthPresenter : BasePresenter<IEntityHealth>
    {
        public HealthPresenter(BaseView<IEntityHealth> view, IEntityHealth model) : base(view, model)
        {
            Model.OnHpChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float hp)
        {
            OnModelChanged();
        }
        
        public override void Dispose()
        {
            Model.OnHpChanged -= OnHealthChanged;
        }
    }
}