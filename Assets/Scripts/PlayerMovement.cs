using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float groundDrag;


    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask whatIsGround;
    bool grounded;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 10.0f;
    [SerializeField]
    private float jumpCooldown = 1f;
    private bool jumpEnabled = true;




    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    private InputManager inputManager;
    

    private void Start()
    {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        inputManager.jumpAction.performed += JumpAction_performed;
    }

    private void JumpAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Jump();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f+0.2f, whatIsGround);

        GetInput();
        SpeedControl();
        //SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

    }
    private void GetInput()
    {
        Vector2 movementInput = inputManager.GetPlayerMovement();
        horizontalInput = movementInput.y;
        verticalInput = movementInput.x;    
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * horizontalInput + orientation.right * verticalInput;
        rb.AddForce (moveDirection.normalized * moveSpeed*10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3 (rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3 (limitedVel.x,rb.velocity.y, limitedVel.z);    

        }
    }
    private IEnumerator ActivateJump()
    {
        yield return new WaitForSeconds(jumpCooldown);
        jumpEnabled = true;
    }
    private void Jump()
    {
        if (grounded && jumpEnabled)
        {
            Debug.Log("Jumping");
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            jumpEnabled = false;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(ActivateJump());
        }
    }
}
