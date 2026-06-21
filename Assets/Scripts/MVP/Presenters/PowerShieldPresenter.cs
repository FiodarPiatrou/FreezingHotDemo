using Entities.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class PowerShieldPresenter : BasePresenter<IEntityPowerShield>
    {
        public PowerShieldPresenter(BaseView<IEntityPowerShield> view, IEntityPowerShield model) : base(view, model)
        {
            Model.OnChargeChanged += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnChargeChanged -= OnModelChanged;
        }
    }
}