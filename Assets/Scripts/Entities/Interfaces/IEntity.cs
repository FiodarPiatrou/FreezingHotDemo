using UnityEngine;

namespace Entities.Interfaces
{
    public interface IEntity
    {
        public T GetEntityComponent<T>() where T : class;
    }
}