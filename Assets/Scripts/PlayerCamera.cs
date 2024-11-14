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

    private float xRotation= 0.0f, yRotation = 0.0f;
    private void Start()
    {
        inputManager = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseDelta = inputManager.GetMouseDelta();
        yRotation += mouseDelta.x;
        xRotation -= mouseDelta.y;


        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation*sensitivityX,yRotation*sensitivityY,0);
        orientation.rotation = Quaternion.Euler(0, yRotation*sensitivityY, 0);
    }
}
