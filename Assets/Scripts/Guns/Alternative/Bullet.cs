using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rigidBody;
    
    private float disableTime = 3;

    [field:SerializeField]
    public Vector3 SpawnLocation
    {
        get; private set;
    }

    public delegate void CollisionEvent(Bullet bullet, Collision col);
    public event CollisionEvent OnCollision;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (rigidBody.velocity != Vector3.zero)
        {
            // Rotar el proyectil para que apunte en la dirección de su velocidad
            transform.rotation = Quaternion.LookRotation(rigidBody.velocity);
        }
    }

    public void Spawn(Vector3 spawnForce)
    {
        SpawnLocation = transform.position;
        transform.forward = spawnForce.normalized;
        rigidBody.AddForce(spawnForce,ForceMode.VelocityChange);
        StartCoroutine(DelayedDisable(disableTime));

    }
    private IEnumerator DelayedDisable (float time)
    {
        yield return new WaitForSeconds(time);
        OnCollisionEnter(null);

    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        OnCollision = null;
    }
}
