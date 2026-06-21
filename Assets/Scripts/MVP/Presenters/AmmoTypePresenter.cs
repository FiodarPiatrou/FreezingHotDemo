using Entities.Interfaces;
using Inventory.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class AmmoTypePresenter: BasePresenter<IEntityWeaponController>
    {
        public AmmoTypePresenter(BaseView<IEntityWeaponController> view, IEntityWeaponController model) : base(view, model)
        {
            Model.OnChangedAmmo += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnChangedAmmo -= OnModelChanged;
        }
        
    }
}