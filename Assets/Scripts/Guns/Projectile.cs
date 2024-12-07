using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    public LayerMask interactionMask;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        Destroy(gameObject, 5);
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject,0.5f);
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            Debug.Log("Colliding with (Hitbox) "+collision.gameObject.name);
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log("Colliding with (default) " + collision.gameObject.name);
            return;
        }
        

    }
}
