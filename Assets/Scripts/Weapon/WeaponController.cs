using System;
using Entities;
using Entities.Interfaces;
using Inventory;
using Inventory.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapon
{
    public class WeaponController : EntityComponent, IEntityWeaponController
    {
        public Vector2 AimDirection { get; private set; }

        private Vector2 _aimInput;
        private PlayerInput _playerInput;
        private InputAction _aimAction;
        [SerializeField] private Entity gunEntity;
        private IEntityGunPosition _gunPosition;
        private IEntityWeaponComponent _weaponComponent;
        private InputSystem_Actions _input;
        [SerializeField] private Transform firepoint;
        private IAmmoInventory _ammoInventory;
        public AmmoType CurrentAmmoType { get; private set; }
        public event Action OnChangedAmmo;

        private void Awake()
        {
            _input = new InputSystem_Actions();
        }

        private void Start()
        {
            _gunPosition = gunEntity.GetEntityComponent<IEntityGunPosition>();
            _weaponComponent = gunEntity.GetEntityComponent<IEntityWeaponComponent>();
            _ammoInventory = Owner.GetEntityComponent<IAmmoInventory>();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Attack.performed += OnAttack;
            _input.Player.ChangeAmmo.performed += OnChangeAmmo;
            
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Attack.performed -= OnAttack;
            _input.Player.ChangeAmmo.performed -= OnChangeAmmo;
        }

        public void OnChangeAmmo(InputAction.CallbackContext ctx)
        {
            
            if (CurrentAmmoType==AmmoType.Fire)
            {
                CurrentAmmoType = AmmoType.Ice;
            }
            else
            {
                CurrentAmmoType = AmmoType.Fire;
            }
            OnChangedAmmo?.Invoke();
        }

        private void FixedUpdate()
        {
            _aimInput = _input.Player.Aim.ReadValue<Vector2>();
            OnAim(_aimInput);
        }

        private void OnAim(Vector2 aimVector)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(aimVector);
            _gunPosition.LookAt(mousePosition);
        }

        private void OnAttack(InputAction.CallbackContext ctx)
        {
            if (_ammoInventory.TryUseAmmo(CurrentAmmoType,1))
            {
                _weaponComponent.Shoot(CurrentAmmoType);
            }
            
            
        }

    }
}