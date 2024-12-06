using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed = 10.0f;
    public float sprintSpeed = 20.0f;
    [SerializeField]
    private float groundDrag;
    private bool activateSprinting = false;


    [Header("Ground Check")]
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask whatIsGround;
    public bool grounded;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 10.0f;
    [SerializeField]
    private float jumpCooldown = 1f;
    private bool jumpEnabled = true;
    private bool jumping = false;
    [SerializeField]
    private float airModifier;


    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope = false;
    [Header("Debug UI")]
    [SerializeField]
    private TMP_Text debuggingText;

    public event EventHandler OnJumpPerformed;

    
    public enum MovementState
    {
        walking = 0,
        sprinting = 1,
        air = 2,
    }
    MovementState movementState;


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
        inputManager.sprintAction.performed += SprintAction_performed;
        inputManager.sprintAction.canceled += SprintAction_canceled;
        inputManager.resetAction.performed += ResetAction_performed;
        debuggingText.text = $"vivimos en una sociedad";
    }

    private void ResetAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Reset");
        transform.position = new Vector3 (50,16,81);
    }

    private void SprintAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Stop Sprint");
        activateSprinting = false;
    }

    private void SprintAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Start Sprint");
        activateSprinting = true;
        
    }

    private void JumpAction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Jump();
    }
    private void StateHandler()
    {
        if( grounded && activateSprinting) // sprinting
        {
            movementState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if( grounded && !activateSprinting)
        {
            movementState= MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else if (!grounded)
        {
            movementState= MovementState.air;
        }
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.4f, whatIsGround);
        StateHandler();
        GetInput();
        SpeedControl();
        //SpeedControl();
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        debuggingText.text = $"Grounded: {grounded}\n" +
                    $"Velocity: {rb.velocity}\n" +
                    $"Speed: {rb.velocity.magnitude.ToString("F2")}\n" +
                    $"OnSlope: {OnSlope()}\n" +
                    $"isExitingSlope: {exitingSlope}\n" +
                    $"SlopeMoveDirection: {GetSlopeMoveDirection()}\n" +
                    $"Position: {transform.position}";
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


        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airModifier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope )
        {
            if( rb.velocity.magnitude > moveSpeed )
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
        
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3 (limitedVel.x,rb.velocity.y, limitedVel.z);
            }
        }
    }
    private IEnumerator ActivateJump()
    {
        yield return new WaitForSeconds(jumpCooldown);
        jumpEnabled = true;
        exitingSlope = false;
    }
    private void Jump()
    {
        exitingSlope = true;
        if (grounded && jumpEnabled)
        {
            OnJumpPerformed?.Invoke(this, EventArgs.Empty);
            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            jumpEnabled = false;
            jumping = true;
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(ActivateJump());

        }
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f +0.3f,whatIsGround) )
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
