using Entities.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class TemperaturePresenter : BasePresenter<IEntityTemperature>
    {
        public TemperaturePresenter(BaseView<IEntityTemperature> view, IEntityTemperature model) : base(view, model)
        {
            Model.OnTemperatureChanged += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnTemperatureChanged -= OnModelChanged;
        }
    }
}