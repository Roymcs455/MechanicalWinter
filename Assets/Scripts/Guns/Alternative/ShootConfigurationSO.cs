using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Configuration", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationSO : ScriptableObject
{
    public bool isHitscan = true;
    public Bullet bulletPrefab;
    public float bulletSpawnForce = 1000;

    public LayerMask hitMask;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float fireRate = 1.0f;


}
