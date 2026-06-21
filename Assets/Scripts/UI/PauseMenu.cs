using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon; // Required for New Input System

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private UIInputManager _uiInputManager;
        private bool _isPaused = false;

        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private PlayerInput playerInput; // Reference to the PlayerInput component

        private WeaponController _shootingScript; 

        private void Start()
        {
            _uiInputManager = ServiceLocator.Instance.Get<UIInputManager>();
            _uiInputManager.PauseToggled += OnPauseToggled;

            // Find the shooting script on the player object
            if (playerInput != null)
            {
                _shootingScript = playerInput.GetComponent<WeaponController>();
            }
        }

        public void Pause()
        {
            _isPaused = true;
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;

            // Switch to the UI action map
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap("UI"); 
            }

            // Disable the shooting script
            if (_shootingScript != null)
            {
                _shootingScript.enabled = false;
            }
        }

        public void Resume()
        {
            _isPaused = false;
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;

            // Switch back to the Player action map
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap("Player");
            }

            // Re-enable the shooting script
            if (_shootingScript != null)
            {
                _shootingScript.enabled = true;
            }
        }

        private void OnPauseToggled()
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1f;

            // Ensure we are back to normal input before leaving
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap("Player");
            }

            // Ensure shooting is re-enabled before leaving
            if (_shootingScript != null)
            {
                _shootingScript.enabled = true;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            if (_uiInputManager != null)
                _uiInputManager.PauseToggled -= OnPauseToggled;
        }
    }
}
