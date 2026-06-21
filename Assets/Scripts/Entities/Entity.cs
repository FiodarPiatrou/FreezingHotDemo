using System;
using System.Collections.Generic;
using Entities.Interfaces;
using UnityEngine;

namespace Entities
{
    public class Entity : MonoBehaviour, IEntity
    {
        private readonly Dictionary<Type, object> _moduleCache = new();
        
        private void Awake()
        {
            var modules = GetComponents<EntityComponent>();
            foreach (var module in modules) RegisterComponent(module);
        }
        
        public T GetEntityComponent<T>() where T : class
        {
            if (_moduleCache.TryGetValue(typeof(T), out var module))
                return module as T;

            var comp = GetComponent<T>();
            
            if (comp != null)
                _moduleCache[typeof(T)] = comp;

            return comp;
        }
        
        private void RegisterComponent(EntityComponent entityModule)
        {
            var moduleType = entityModule.GetType();

            if (!_moduleCache.TryAdd(moduleType, entityModule)) return;

            var interfaces = moduleType.GetInterfaces();
            foreach (var face in interfaces) _moduleCache.TryAdd(face, entityModule);
            entityModule.Initialize(this);
        }
    }
}
