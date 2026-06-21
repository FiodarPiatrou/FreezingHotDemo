using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance { get; private set; }
        
        [SerializeField] private List<BaseManager> managersList;
        
        private readonly Dictionary<Type, BaseManager> _cachedServices = new Dictionary<Type, BaseManager>();

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                RegisterAllManagers();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void RegisterAllManagers()
        {
            foreach (var manager in managersList)
            {
                if (!manager) continue;

                var type = manager.GetType();

                _cachedServices.TryAdd(type, manager);
                
            }
        }
        
        public T Get<T>() where T : BaseManager
        {
            var type = typeof(T);

            if (_cachedServices.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            
            return null;
        }
    }
}