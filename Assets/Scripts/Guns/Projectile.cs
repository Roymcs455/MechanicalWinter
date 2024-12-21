using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public LayerMask interactionMask;
    public LayerMask ground;
    public LayerMask hitboxLayer;

    public float explosionRadius = 5.0f;
    public float explosionPower = 10.0f;
    public DamageTypes damageType;
    public float damage = 0.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Destroy(gameObject, 15);
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
        
        if (IsInLayerMask(collision.gameObject, interactionMask | hitboxLayer | ground))
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius,interactionMask | hitboxLayer);
            foreach (Collider hit in colliders)
            {
                Rigidbody hitRB = hit.GetComponent<Rigidbody>();
                float distance = (hit.transform.position - explosionPos).magnitude;
                Damage appliedDamage = new Damage(DamageTypes.EXPLOSIVE, damage * distance / explosionRadius);

                ArmorPiece hitArmorPiece = hit.gameObject.GetComponent<ArmorPiece>();
                if (hitArmorPiece != null)
                    hitArmorPiece.ReceiveDamage(appliedDamage,gameObject.GetInstanceID());
                
                if (hitRB != null)
                {
                    hitRB.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F,ForceMode.Impulse);
                    //Debug.Log($"Exploding {hit.gameObject.name}");
                }
                
            }
            Destroy(gameObject);
        }
        

    }
    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}
