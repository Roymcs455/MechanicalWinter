using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void ReceiveDamage(Damage appliedDamage, int projectileID = 0);
    
}
