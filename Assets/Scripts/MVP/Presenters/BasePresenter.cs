using System;
using Entities;
using MVP.Views;

namespace MVP.Presenters
{
    public abstract class BasePresenter<T> where T : class
    {
        private readonly BaseView<T> _view;
        protected readonly T Model;

        protected readonly Action OnModelChanged;

        protected BasePresenter(BaseView<T> view, T model)
        {
            _view = view;
            Model = model;

            OnModelChanged = () => _view.OnModelChanged(Model);
            _view.OnModelChanged(Model);
        }

        public abstract void Dispose();
    }
}