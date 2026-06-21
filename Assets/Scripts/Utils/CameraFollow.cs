using UnityEngine;

namespace Utils
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private Vector3 offset = new(0, 0, -10);
        
        [Range(0,180)] [SerializeField] private float fieldOfView = 60;

        private Camera _camera;
        private Vector3 _velocity = Vector3.zero;

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        private void FixedUpdate()
        {
            if (!target)
                return;

            if(!Mathf.Approximately(fieldOfView, _camera.fieldOfView))
                _camera.fieldOfView = fieldOfView;
            
            var targetPosition = target.position + offset;


            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }
    }
}
