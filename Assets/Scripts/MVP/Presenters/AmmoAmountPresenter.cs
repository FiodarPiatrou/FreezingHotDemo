using Inventory.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{ 
        public class AmmoAmountPresenter : BasePresenter<IAmmoInventory>
        {
                public AmmoAmountPresenter(BaseView<IAmmoInventory> view, IAmmoInventory model) : base(view, model)
                {
                        Model.OnAmmoAmountChanged += OnModelChanged;
                }

                public override void Dispose()
                {
                        Model.OnAmmoAmountChanged -= OnModelChanged;
                }
        }
}
