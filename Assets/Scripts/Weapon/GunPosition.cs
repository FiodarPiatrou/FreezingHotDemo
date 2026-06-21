using System;
using Entities;
using Entities.Interfaces;
using UnityEngine;

namespace Weapon
{
    
public class GunPosition : EntityComponent, IEntityGunPosition
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool isBehind=false;
    
    public void LookAt(Vector3 target)
    {
        float lookAngle = AngleBetweenTwoPoints(gunTransform.position,  target)+90f;
        
        gunTransform.eulerAngles=new Vector3(0, 0, lookAngle+90f);
        var eulerAngle = gunTransform.eulerAngles.z;
        if (eulerAngle is >= 90 and < 270 && isBehind)
        {
            isBehind = false;
            //_spriteRenderer.sortingOrder = 1;
            _spriteRenderer.flipY = false;
        }
        else if (eulerAngle is >=270 or < 90 && !isBehind)
        {
            isBehind = true;
            //_spriteRenderer.sortingOrder = -1;
            _spriteRenderer.flipY = true;
        }
        
        
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x)*Mathf.Rad2Deg;
    }

    
}
}
