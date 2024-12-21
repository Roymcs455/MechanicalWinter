using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueAim : MonoBehaviour
{
    [SerializeField]
    private Transform aimPosition;
    [SerializeField]
    private Transform cameraRotation;
    [SerializeField]    
    public float rotationSpeed = 10.0f;

    //public float randomnessX = 1.0f;
    //public float randomnessY = 1.0f;
    //public float randomnessZ = 1.0f;

    private void Update()
    {

        //Quaternion randomness = Quaternion.Euler(
        //    Random.Range(-randomnessX,randomnessX), 
        //    Random.Range(-randomnessY, randomnessY),
        //    Random.Range(-randomnessZ, randomnessZ)
        //);
        transform.position = aimPosition.position;
        //transform.rotation = Quaternion.RotateTowards( transform.rotation , cameraRotation.rotation * randomness , Time.deltaTime*rotationSpeed );
        transform.rotation = Quaternion.RotateTowards( transform.rotation , cameraRotation.rotation , Time.deltaTime*rotationSpeed );
        
    }
}
