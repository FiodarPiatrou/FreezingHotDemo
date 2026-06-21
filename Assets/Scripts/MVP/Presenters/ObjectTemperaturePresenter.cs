using Environment.Interfaces;
using MVP.Views;

namespace MVP.Presenters
{
    public class ObjectTemperaturePresenter : BasePresenter<ITemperatureZone>
    {
        public ObjectTemperaturePresenter(BaseView<ITemperatureZone> view, ITemperatureZone model) : base(view, model)
        {
        }

        public override void Dispose()
        {
        }
    }
}