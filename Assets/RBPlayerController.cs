using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RBPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject camHolder;
    public float speed, sensitivity, maxForce, jumpForce;
    private Vector2 move, look;
    private float lookRotation;
    public bool grounded = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if(grounded)
        {
            jumpForces= Vector3.up * jumpForce;

        }
        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }
    void Move()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - currentVelocity);

        velocityChange = new Vector3 (velocityChange.x, 0, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);


        rb.AddForce(velocityChange, ForceMode.VelocityChange);

    }
    private void Look()
    {
        transform.Rotate(Vector3.up * look.x * sensitivity);

        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        Look();
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }

}
