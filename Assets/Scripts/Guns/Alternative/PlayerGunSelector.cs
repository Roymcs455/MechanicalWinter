using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType gunType;
    [SerializeField]
    private Transform gunParent;
    [SerializeField]
    private List<GunSO> guns;
    [Space]

    public GunSO activeGun;


    private void Start()
    {
        SetGun();
    }

    private void SetGun()
    {
        GunSO gun = guns.Find(gun => gun.gunType == gunType);
        if (gun == null)
        {
            Debug.LogError($"Gun {gunType} not found");
            return;
        }
        activeGun = gun;
        gun.Spawn(gunParent, this);
    }

    public void ChangeWeapon(GunType gunType)
    {
        
        activeGun.Despawn();
        this.gunType = gunType;
        SetGun();
    }
}
