using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bullet;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public LayerMask interactionMask;
    public LayerMask ground;
    public LayerMask hitboxLayer;
    
    public bool explosive;
    public float explosionRadius = 5.0f;
    public float explosionPower = 10.0f;
    public DamageTypes damageType;
    public float damage = 50.0f;
    public float disableTime;

    public delegate void CollisionEvent(Projectile bullet, Collision col);
    public event CollisionEvent OnCollision;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //Destroy(gameObject, 15);
    }
    private void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            // Rotar el proyectil para que apunte en la dirección de su velocidad
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        OnCollision?.Invoke(this, collision);
          

    }

    private void ContactDamage(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.TryGetComponent(out IDamageable damageable))
            {
                damageable.ReceiveDamage(new Damage(damageType, damage));
            }
        }
    }

    private void Explosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius, interactionMask | hitboxLayer);
        foreach (Collider hit in colliders)
        {
            Rigidbody hitRB = hit.GetComponent<Rigidbody>();
            float distance = Vector3.Distance(hit.transform.position, explosionPos);
            if(hit.TryGetComponent(out IDamageable damageable) )
            {
                damageable.ReceiveDamage(new Damage(damageType, damage * distance / explosionRadius));
            }
            if (hitRB != null)
            {
                hitRB.AddExplosionForce(explosionPower/2, explosionPos, explosionRadius, explosionPower / 2, ForceMode.Impulse);
                //Debug.Log($"Exploding {hit.gameObject.name}");
            }
        }    
    }
    public void Spawn(Vector3 spawnForce)
    {
        transform.forward = spawnForce.normalized;
        rb.AddForce(spawnForce, ForceMode.VelocityChange);
        StartCoroutine(DelayedDisable(disableTime));

    }
    private IEnumerator DelayedDisable(float time)
    {
        yield return new WaitForSeconds(time);
        OnCollisionEnter(null);

    }
    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
