using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : MonoBehaviour,GunInterface
{
    //Public
    public GameObject bullet;
    

    public float shootForce, upwardForce;

    public float shootCooldown, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;

    public bool allowButtonHold;
    InputManager inputManager;

    
    bool shooting, readyToShoot = true, reloading;
    
    public Transform attackPoint;

    private void FireAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        shooting = false;
        Debug.Log("Firing Canceled");
    }

    private void FireAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        shooting = true;
        Debug.Log("Firing Performed");
        
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.fireAction.performed += FireAction_performed;
        inputManager.fireAction.canceled += FireAction_canceled;

        
        readyToShoot = true;
    }

    private void Update()
    {
        if (shooting == true)
            Shoot();
    }
    public void Shoot()
    {
        if (readyToShoot)
        {
            readyToShoot = false;
            Vector3 direction = transform.forward;  
            float spreadX = Random.Range(-spread/100, spread/100);
            float spreadY = Random.Range(-spread / 100, spread / 100);
            Vector3 directionWithSpread = direction + new Vector3(spreadX, spreadY,0);
            directionWithSpread.Normalize();
            GameObject currentBullet = Instantiate(bullet, attackPoint.position, transform.rotation );
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread * shootForce, ForceMode.VelocityChange);

            Invoke("ResetShoot", shootCooldown);
            if(!allowButtonHold)
                shooting = false;
        }
    }
    private void OnDestroy()
    {
        inputManager.fireAction.performed -= FireAction_performed;
        inputManager.fireAction.canceled -= FireAction_canceled;
    }
    private void ResetShoot() { readyToShoot = true; }
}
