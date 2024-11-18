using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera: MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField]
    private float sensitivityX=1.0f, sensitivityY =1.0f;
    [SerializeField]
    private Transform orientation;
    [SerializeField]
    private float clampMin = -90.0f, clampMax=90.0f;
    [Header("Debug")]
    [SerializeField]
    Transform point;
    [SerializeField]
    float crosshairDistance=100f;

    private float xRotation= 0.0f, yRotation = 0.0f;
    private void Start()
    {
        inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        point.position = transform.position+transform.up*10;
    }

    void Update()
    {
        Vector2 mouseDelta = inputManager.GetMouseDelta();
        yRotation += mouseDelta.x*sensitivityY;
        xRotation -= mouseDelta.y*sensitivityX;


        xRotation = Mathf.Clamp(xRotation, clampMin, clampMax);
        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        RaycastHit target; 
        if(Physics.Raycast(transform.position, transform.forward, out target, crosshairDistance))
        {
            
            point.position = target.point;

        }
        else
        {
            
        }

    }
}
