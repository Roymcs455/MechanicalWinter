using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private PlayerControls playerControls;
    [Header("Input Actions Map")]
    public InputActionAsset inputActions;
    public InputAction jumpAction, sprintAction, fireAction, resetAction;
    

    public static InputManager Instance {  get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
        jumpAction = inputActions.FindAction("Jump");       
        jumpAction.Enable();
        sprintAction = inputActions.FindAction("Sprint");   
        sprintAction.Enable();
        fireAction = inputActions.FindAction("Fire");       
        fireAction.Enable();
        resetAction = inputActions.FindAction("Reset"); 
        resetAction.Enable();

    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }
    public bool PlayerJumpedThisFrame()
    {
        
        return playerControls.Player.Jump.ReadValue<bool>();
    }
    //public bool PlayerShootThisFrame()
    //{
    //    return playerControls.Player.Fire.ReadValue<bool>();
    //}
}
