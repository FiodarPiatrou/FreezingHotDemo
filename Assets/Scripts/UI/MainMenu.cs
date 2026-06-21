using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            // Load the main game scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    
        public void QuitGame()
        {
            // Quit the application
            Application.Quit();
        }
    }
}
