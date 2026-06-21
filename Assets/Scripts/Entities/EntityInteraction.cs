using System;
using System.Collections.Generic;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class EntityInteraction : EntityComponent
    {
        private readonly List<IInteractable> _objectsInRange = new List<IInteractable>();
        private InputSystem_Actions _input;
        private IInteractable _currentTarget;

        private void Awake()
        {
            _input = new InputSystem_Actions();
        }
        
        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Interact.performed += OnInteractPerformed;
        }

        private void OnDisable()
        {
            if (_currentTarget != null) 
                _currentTarget.SetFocused(false);
                
            if (_input != null)
            {
                _input.Player.Interact.performed -= OnInteractPerformed;
                _input.Disable();
                _input.Dispose();
            }
        }

        private void Update()
        {
            if (_objectsInRange.Count == 0) return;

            IInteractable bestTarget = null;
            float closestDistance = float.MaxValue;

            for (int i = _objectsInRange.Count - 1; i >= 0; i--)
            {
                var item = _objectsInRange[i];

                if (item == null || item.Equals(null)) 
                {
                    _objectsInRange.RemoveAt(i);
                    continue;
                }

                var mono = item as MonoBehaviour;
                if (!mono) continue;

                float distance = Vector2.Distance(transform.position, mono.transform.position);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = item;
                }
            }

            if (_currentTarget != bestTarget)
            {
                if (_currentTarget != null) _currentTarget.SetFocused(false);
                
                _currentTarget = bestTarget;
                
                if (_currentTarget != null) _currentTarget.SetFocused(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                if (!_objectsInRange.Contains(interactable))
                {
                    _objectsInRange.Add(interactable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                if (_objectsInRange.Contains(interactable))
                {
                    if (_currentTarget == interactable)
                    {
                        interactable.SetFocused(false);
                        _currentTarget = null;
                    }
                    _objectsInRange.Remove(interactable);
                }
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (_currentTarget != null)
            {
                _currentTarget.Interact(Owner);
            }
        }
    }
}