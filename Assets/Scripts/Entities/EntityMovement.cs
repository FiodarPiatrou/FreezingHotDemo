using Enums;
using Managers;
using Player;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EntityMovement : EntityComponent
    {
        [Header("Audio")]
        [SerializeField] private SoundId footstepSoundId = SoundId.Footstep;
        [SerializeField] private float footstepInterval = 0.5f;
        [SerializeField] private bool playFootstepSounds = true;
        
        
        private float _moveSpeed = 1f;
        private float _footstepTimer;
        
        protected Rigidbody2D Rb;
        protected Vector2 MoveInput;
        private AudioManager _audioManager;
        protected bool IsMoving;
        
        
        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            AwakeInitialize();
        }
        
        void Start()
        {
            _moveSpeed = Owner.GetEntityComponent<EntityStatsProvider>().EntityStats.Speed;
            _audioManager = ServiceLocator.Instance.Get<AudioManager>();
        }
        
        private void Update()
        {
            // Flip entity based on movement direction
            if (Mathf.Abs(MoveInput.x) > Mathf.Epsilon)
            {
                Flip();
            }
            
            if (!playFootstepSounds || !_audioManager) 
                return;
            
            if (IsMoving)
            {
                _footstepTimer += Time.deltaTime;

                if (_footstepTimer >= footstepInterval)
                {
                    _audioManager.PlaySfx(footstepSoundId);
                    _footstepTimer -= footstepInterval;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }
        }
        
        protected abstract void Flip();
        
        private void FixedUpdate()
        {
            MoveEntity();
        }

        protected virtual void MoveEntity()
        {
            if (!Rb) return;
            Vector2 nextPos = Rb.position + MoveInput * (_moveSpeed * Time.fixedDeltaTime);
            Rb.MovePosition(nextPos);
        }

        protected abstract void AwakeInitialize();
    }
}
