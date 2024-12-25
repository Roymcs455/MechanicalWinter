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
    public DamageTypes type;
    public float value;
    public Damage(DamageTypes damageType, float damage)
    {
        this.type = damageType;
        this.value = damage;
    }

}