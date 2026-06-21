using System;
using UnityEngine.InputSystem;

namespace Managers
{
    public class UIInputManager : BaseManager
    {
        
        private InputSystem_Actions _input; 
        private InputAction _pauseAction;

        public event Action PauseToggled;

        private void Awake()
        {
            _input = new InputSystem_Actions();
            _pauseAction = _input.Player.Pause;
        }
    
        private void OnEnable()
        {
            _input.Enable();
            _pauseAction.performed += OnPausePerformed;
        }
    
        private void OnDisable()
        {
            _pauseAction.performed -= OnPausePerformed;
            _input.Disable(); 
        }

        private void OnPausePerformed(InputAction.CallbackContext obj)
        {
            PauseToggled?.Invoke();
        }
    }
}