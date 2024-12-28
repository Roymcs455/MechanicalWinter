using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;
    [SerializeField]
    private AnimationHandler playerAnimationHandler;
    private int weapon;
    private void Start()
    {
        weapon = 0;
        gunSelector.activeGun.ShootEvent += ActiveGun_ShootEvent;
    }

    private void ActiveGun_ShootEvent()
    {
        playerAnimationHandler.Shoot();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null)
        {
            gunSelector.activeGun.Shoot();
            
        }
        if (Mouse.current.rightButton.isPressed)
        {
            weapon++;
            gunSelector.activeGun.ShootEvent -= ActiveGun_ShootEvent;
            gunSelector.ChangeWeapon((GunType)(weapon%2));
            gunSelector.activeGun.ShootEvent += ActiveGun_ShootEvent;
        }
        
    } 
}
