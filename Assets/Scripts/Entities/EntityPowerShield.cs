using System;
using System.Collections;
using Enemy.Interfaces;
using Entities.Interfaces;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Entities
{
    public class EntityPowerShield : EntityComponent, IEntityPowerShield
    {
        [Header("Visual Settings")]
        [SerializeField] private float shockWaveTime = 0.75f;
        [SerializeField] private GameObject shockWavePrefab;
        
        [Header("Physics Settings")]
        [SerializeField] private float pushForce = 15f;
        [SerializeField] private float pushRadius = 3f;
        [SerializeField] private LayerMask enemyLayer;
        
        private const int MaxEnemiesToPush = 20; 

        [Header("Recharge Settings")]
        [SerializeField] private float rechargeSpeed = 0.2f;

        private Coroutine _shockWaveCoroutine;
        private Material _shockWaveMaterial;
        private static readonly int WaveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
        
        private InputSystem_Actions _input;
        private float _charge = 1f;

     
        private readonly Collider2D[] _resultsBuffer = new Collider2D[MaxEnemiesToPush];
        private ContactFilter2D _filter;

        public float Charge => _charge;
        public event Action OnChargeChanged;

        private void Awake()
        {
       
            if (shockWavePrefab.TryGetComponent(out SpriteRenderer sr))
            {
                _shockWaveMaterial = sr.material;
            }

      
            _filter = new ContactFilter2D();
            _filter.SetLayerMask(enemyLayer); 
            _filter.useLayerMask = true;     
            _filter.useTriggers = false;      


            _input = new InputSystem_Actions();
  
            shockWavePrefab.SetActive(false);
        }
        
        private void OnEnable()
        {
            _input.Enable(); 
            _input.Player.PowerShield.performed += OnPowerShield;
        }

        private void OnDisable()
        {
            _input.Disable(); 
            _input.Player.PowerShield.performed -= OnPowerShield;
            if (_shockWaveMaterial) Destroy(_shockWaveMaterial);
        }

        private void Update()
        {
            if (_charge < 1)
            {
                _charge += Time.deltaTime * rechargeSpeed;
                if (_charge > 1) _charge = 1; 
                OnChargeChanged?.Invoke();
            }
        }

        private void OnPowerShield(InputAction.CallbackContext value)
        {
            if (_charge < 1) return;
            
            CallShockWave();
            ApplyRepulsiveForce(); 
            
            _charge = 0;
            ServiceLocator.Instance.Get<AudioManager>().PlaySfx(SoundId.Shield);
            OnChargeChanged?.Invoke();
        }
        
        private void ApplyRepulsiveForce()
        {
          
            int hitCount = Physics2D.OverlapCircle(
                transform.position, 
                pushRadius, 
                _filter, 
                _resultsBuffer
            );

            for (int i = 0; i < hitCount; i++)
            {
                var target = _resultsBuffer[i];
                if (!target) continue;

                Vector2 direction = (target.transform.position - transform.position).normalized;
                if (direction == Vector2.zero) direction = UnityEngine.Random.insideUnitCircle.normalized;
        
                Vector2 finalForce = direction * pushForce;
                
                if (target.TryGetComponent(out IKnockbackable knockbackable))
                {
                    knockbackable.ApplyKnockback(finalForce);
                }
                else if (target.TryGetComponent(out Rigidbody2D rb))
                {
                    rb.AddForce(finalForce, ForceMode2D.Impulse);
                }
            }
        }

        private void CallShockWave()
        {
            if (_shockWaveCoroutine != null) StopCoroutine(_shockWaveCoroutine);
            _shockWaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1f));
        }

        private IEnumerator ShockWaveAction(float startPos, float endPos)
        {
            shockWavePrefab.SetActive(true);
            _shockWaveMaterial.SetFloat(WaveDistanceFromCenter, startPos);
            
            float elapsedTime = 0f;

            while (elapsedTime < shockWaveTime)
            {
                elapsedTime += Time.deltaTime;
                float lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime / shockWaveTime);
                _shockWaveMaterial.SetFloat(WaveDistanceFromCenter, lerpedAmount);
                yield return null;
            }
            
            _shockWaveMaterial.SetFloat(WaveDistanceFromCenter, endPos);
            shockWavePrefab.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, pushRadius);
        }
    }
}
