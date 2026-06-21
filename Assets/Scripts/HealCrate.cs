using System;
using Entities;
using Entities.Interfaces;
using UnityEngine;


public class HealCrate : EntityComponent, IInteractable
{
    [SerializeField] private float healAmount;
    private bool _isFocused = false;
    

    public void Interact(IEntity entity)
    {
        entity.GetEntityComponent<IEntityHealth>().Heal(healAmount);
        Destroy(gameObject);
    }

    public void SetFocused(bool value)
    {
        Debug.Log("set focused");
        _isFocused = value;
        OnObjectChanged?.Invoke();
    }

    public string DisplayName => "Press [E] to heal";
    public bool IsFocused => _isFocused;
    public event Action OnObjectChanged;
}
