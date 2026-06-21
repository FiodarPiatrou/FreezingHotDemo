using System;

namespace Entities.Interfaces
{
    public interface IInteractable
    {
        public void Interact(IEntity entity);
        
        public void SetFocused(bool value);
        
        public string DisplayName { get; }
        
        public bool IsFocused { get; }
        public event Action OnObjectChanged;
    }
}