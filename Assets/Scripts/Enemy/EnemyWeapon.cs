using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spawnPoint;
    public GameObject weaponPrefab;
    public Bullet projectile;
    public Vector3 weaponOffset;
    public Vector3 weaponRotation;
    public Transform weaponHandle;

    public LayerMask whatIsPlayer;
    public LayerMask obstacles;

    private float lastShootTime;

    

    void Shoot()
    {
        //if (IsInLayerMask(collision.gameObject, interactionMask | hitboxLayer | ground))
        //{
        //    if (explosive)
        //    {
        //        Explosion();

        //    }
        //    else
        //    {
        //        ContactDamage(collision);
        //    }
        //}
    }

}
