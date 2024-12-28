using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PATROL,
        ATTACK,
    }
    private const string IS_WALKING = "IsWalking";
    private const string JUMP = "Jump";
    private const string LAND = "Land";


    public State agentState;
    public float stoppingDistance;
    private Transform target;
    private Transform viewTransform;
    private Transform pointTransform;
    private Vector3 pointOriginalPosition;
    public float updateSpeed = 0.20f;

    public float viewAngle = 120.0f;
    public float ShootAngle = 60.0f;

    private float health = 100.0f;
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    private bool jumping = false;

    private bool standing = false;
    private float rotationSpeed = 30.0f;
    public Rig rig;
    private Material pointerMaterial;
    private bool playerInSight = false;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        StartCoroutine(FollowTarget());
        agentState = State.IDLE;
        viewTransform = gameObject.transform.Find("View");
        pointTransform = gameObject.transform.Find("Pointer");
        pointOriginalPosition = pointTransform.position;
        Renderer rend = pointTransform.gameObject.GetComponent<Renderer>();
        if (rend == null)
            return;
        if (rend.material == null) return;
        pointerMaterial = rend.material;
        rend.material.color = Color.white;  
        Debug.Log("exito");

    }
    private void Update()
    {
        
        HandleAnimations();
        if(standing)
            LookAtTarget();

    }

    private void HandleAnimations()
    {
        animator.SetBool(IS_WALKING, (agent.velocity.magnitude > 0.01f));

        if (agent.isOnOffMeshLink & !jumping)
        {
            animator.SetTrigger(JUMP);
            jumping = true;
        }
        if (!agent.isOnOffMeshLink & jumping)
        {
            animator.SetTrigger(LAND);
            jumping = false;
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);
        while (enabled) 
        {
            
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.position) <= agent.stoppingDistance)
                {
                    standing = true;
                    
                }
                else
                {
                    standing = false;
                }
                if (CanSeePlayer())
                {
                    playerInSight= true;
                    agent.stoppingDistance = 0;
                    agent.SetDestination(target.position);
                }
                else
                {
                    agent.stoppingDistance = stoppingDistance;
                    playerInSight = false;
                }
            }
            yield return wait;
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    private void LookAtTarget()
    {
        if (target != null)
        {
            Vector3 lookDirection = target.position - transform.position;
            lookDirection.y = 0;

            Quaternion rotation  = Quaternion.LookRotation( lookDirection );
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * rotationSpeed);


        }
    }
    private bool CanSeePlayer()
    {
        
        if (target != null)
        {
            Vector3 directionToPlayer = (target.position - viewTransform.position).normalized;
            float angleBetween = Vector3.Angle(transform.forward, directionToPlayer);
            
            if(angleBetween < viewAngle/2)
            {
                
                if(Physics.Raycast(viewTransform.position, directionToPlayer, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        rig.weight = 1.0f;
                        pointTransform.position = hit.point;
                        pointerMaterial.color = new Color(1,0,0,1);
                        return true;
                    }

                }
            }
        }
        pointerMaterial.color = new Color(0, 0, 1, 1);
        rig.weight = 0;

        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            target = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = null;
        }
    }
    public void AgentIsDead()
    {
        gameObject.SetActive(false);
    }
}
