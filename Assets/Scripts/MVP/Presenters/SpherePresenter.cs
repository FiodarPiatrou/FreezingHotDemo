using Inventory.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class SpherePresenter : BasePresenter<ISphereInventory>
    {
        public SpherePresenter(BaseView<ISphereInventory> view, ISphereInventory model) : base(view, model)
        {
            Model.OnSphereAdded += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnSphereAdded -= OnModelChanged;
        }
    }
}