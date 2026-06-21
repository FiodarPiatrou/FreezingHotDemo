using System;
using Entities;
using Entities.Interfaces;
using Enums;
using Inventory;
using Managers;
using UnityEngine;

namespace Weapon
{
    public class WeaponComponent : EntityComponent, IEntityWeaponComponent
    {
        [SerializeField] private GameObject FireBulletPrefab;
        [SerializeField] private GameObject IceBulletPrefab;
        [SerializeField] private Transform _firepointTransform;
        [SerializeField] private float _bulletSpeed;
    
        private AudioManager _audioManager;
        private void Start()
        {
            _audioManager = ServiceLocator.Instance.Get<AudioManager>();
        }

        public void Shoot(AmmoType ammoType)
        {
            
            GameObject bullet = null;
            if (ammoType==AmmoType.Fire) bullet= Instantiate(FireBulletPrefab,_firepointTransform.position,_firepointTransform.rotation);
            if (ammoType == AmmoType.Ice) bullet= Instantiate(IceBulletPrefab,_firepointTransform.position,_firepointTransform.rotation);
            if (bullet)
            {
                var rb = bullet.GetComponent<Rigidbody2D>();
                rb.linearVelocity = _firepointTransform.right * _bulletSpeed;
            }

            _audioManager.PlaySfx(SoundId.Shoot);
        }
    }
}