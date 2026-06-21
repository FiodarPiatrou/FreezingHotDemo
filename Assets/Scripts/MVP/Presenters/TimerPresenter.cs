using Managers.Interface;
using MVP.Views;

namespace MVP.Presenters
{
    public class TimerPresenter : BasePresenter<ITimer>
    {
        public TimerPresenter(BaseView<ITimer> view, ITimer model) : base(view, model)
        {
            Model.OnTimeChanged += OnModelChanged;
        }

        public override void Dispose()
        {
            Model.OnTimeChanged -= OnModelChanged;
        }
    }
}