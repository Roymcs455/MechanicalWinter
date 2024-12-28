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
    private InputManager inputManager;
    private void Start()
    {
        weapon = 0;
        gunSelector.activeGun.ShootEvent += ActiveGun_ShootEvent;
        inputManager = InputManager.Instance;
        inputManager.changeWeapon.performed += ChangeWeapon_performed;
    }

    private void ChangeWeapon_performed(InputAction.CallbackContext obj)
    {
        ChangeWeapons();
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
        //if (Mouse.current.rightButton.isPressed)
        //{
        //    ChangeWeapons();
        //}

    }

    private void ChangeWeapons()
    {
        weapon++;
        gunSelector.activeGun.ShootEvent -= ActiveGun_ShootEvent;
        gunSelector.ChangeWeapon((GunType)(weapon % 2));
        gunSelector.activeGun.ShootEvent += ActiveGun_ShootEvent;
    }
}
