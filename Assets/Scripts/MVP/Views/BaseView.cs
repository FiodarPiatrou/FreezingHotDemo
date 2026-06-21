using Entities;
using Entities.Interfaces;
using MVP.Presenters;
using UnityEngine;
using UnityEngine.Serialization;

namespace MVP.Views
{
    public abstract class BaseView<T> : MonoBehaviour where T : class
    {
        [SerializeField] private Entity entity;

        protected BasePresenter<T> Presenter;

        private void OnDestroy()
        {
            Presenter.Dispose();
        }

        public abstract void OnModelChanged(T model);

        public T GetModel()
        {
            return entity.GetEntityComponent<T>();
        }
    }
}