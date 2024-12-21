using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    ANTIMATERIAL,
    EXPLOSIVE,
    ENERGY
}
public class Damage
{
    public DamageTypes damageType;
    public float damage;
    public Damage(DamageTypes damageType, float damage)
    {
        this.damageType = damageType;
        this.damage = damage;
    }

}