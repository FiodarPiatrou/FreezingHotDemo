using UnityEngine;

namespace Managers
{
    public class CameraShakeManager : BaseManager
    {
        private float _shakeTimer;
        private float _shakeMagnitude;
        private float _dampingSpeed;
        private Vector3 _initialPos;

      
        private bool _isShaking = false;

        
        private void OnEnable()
        {
            _initialPos = transform.localPosition;
        }

        public void Shake(float duration, float magnitude)
        {
            float calculatedDamping = magnitude / duration;
            Shake(duration, magnitude, calculatedDamping);
        }

        public void Shake(float duration, float magnitude, float damping)
        {
            _shakeTimer = duration;
            _shakeMagnitude = magnitude;
            _dampingSpeed = damping;
            _isShaking = true;
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
             
                transform.localPosition = _initialPos + Random.insideUnitSphere * _shakeMagnitude;
                
                _shakeTimer -= Time.deltaTime;

                
                _shakeMagnitude = Mathf.MoveTowards(_shakeMagnitude, 0f, _dampingSpeed * Time.deltaTime);
            }
            else if (_isShaking)
            {
                _isShaking = false;
                transform.localPosition = _initialPos;
            }
        }
    }
}
