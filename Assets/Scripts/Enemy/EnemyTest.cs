using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour, IDamageable
{
    public void ReceiveDamage(Damage appliedDamage, int projectileID = 0)
    {
        Debug.Log($"I {gameObject.GetInstanceID()} got {appliedDamage.value} of {appliedDamage.type} from {projectileID}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
