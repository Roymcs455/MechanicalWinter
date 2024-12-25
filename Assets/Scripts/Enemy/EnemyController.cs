using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public Transform target;
    private NavMeshAgent agent;

    public void ReceiveDamage(Damage appliedDamage, int projectileID = 0)
    {
        Debug.Log($"I {gameObject.GetInstanceID()} got {appliedDamage.value} of {appliedDamage.type} from {projectileID}");
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = target.position;
    }
}
