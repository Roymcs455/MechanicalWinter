using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform spawnPoint;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public Vector3 muzzleOffset;
    
    public Transform weaponHandle;

    public LayerMask interact;
    

    public float bulletSpeed;

    private GameObject weaponInstance;
    
    
    private ParticleSystem muzzleFlash;

    public void Start()
    {
        weaponInstance = Instantiate(weaponPrefab, weaponHandle);
        spawnPoint = weaponInstance.transform.Find("MuzzleFlash");
        muzzleFlash = spawnPoint.GetComponent<ParticleSystem>();   

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        if (Time.time > lastShootTime + shootDelay)
    //        {
    //            Shoot();
    //        }
    //    }
    //}
    public void Shoot()
    {
        GameObject newBullet = Instantiate(projectilePrefab,spawnPoint.position, spawnPoint.rotation);
        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        newBullet.transform.forward = spawnPoint.forward;
        newBullet.transform.Translate(muzzleOffset);
        bulletRb.AddForce(spawnPoint.forward * bulletSpeed, ForceMode.Impulse);
        newBullet.TryGetComponent(out Projectile newProjectile);
        newProjectile.interactionMask = interact;

        muzzleFlash.Play();




    }

}
