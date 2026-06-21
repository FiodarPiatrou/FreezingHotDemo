using System.Collections;
using Entities;
using Entities.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class DeathScreenUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 1.5f;
        
    
        [SerializeField] protected Entity entity;
        
        private bool _isActive;
        

        private void Awake()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            
        }

        private void Start()
        {
            if (entity)
            {
                var health = entity.GetEntityComponent<IEntityHealth>();
                if (health != null)
                {
                    health.OnDeath += ShowScreen;
                }
                
            }
        }

        protected virtual void ShowScreen()
        {
            if(!_isActive)
                StartCoroutine(FadeInRoutine());
        }

        private IEnumerator FadeInRoutine()
        {

            _isActive = true;
            yield return new WaitForSeconds(0.5f);

            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = timer / fadeDuration;
                yield return null;
            }

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        
            Time.timeScale = 0; 
        }

        public void BackToMenu()
        {
            Time.timeScale = 1; 
            SceneManager.LoadScene("MainMenu");
        }
    
    }
}
