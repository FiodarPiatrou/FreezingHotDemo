using System;
using System.Collections;
using Entities.Interfaces;
using Stats;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletComponent : MonoBehaviour
{
    [SerializeField] private DamageType type;
    [SerializeField] private float damage;
    
    void Start()
    {
        Destroy(gameObject, 1f);

    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEntityHealth>().TakeDamage(type,damage);
            Destroy(gameObject);
        }
        
       
    }
}
