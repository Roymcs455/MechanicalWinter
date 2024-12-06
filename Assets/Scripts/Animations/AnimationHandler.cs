using UnityEngine;
public class AnimationHandler : MonoBehaviour
{
    // Referencia al componente Animator
    private Animator animator;

    // Constantes para los nombres de los parámetros del Animator
    private const string isWalking = "IsWalking";
    private const string isJumping = "IsJumping";
    private const string shoot = "Shoot";
    private const string isOnGround = "IsOnGround";

    [SerializeField] PlayerMovement playerMovementController;

    void Start()
    {
        // Obtener el componente Animator en el GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No se encontró un componente Animator en este GameObject.");
        }
        if (playerMovementController!= null)
        {
            playerMovementController.OnJumpPerformed += PlayerMovementController_OnJumpPerformed;
        }
    }

    private void PlayerMovementController_OnJumpPerformed(object sender, System.EventArgs e)
    {
        animator.SetTrigger(isJumping);
    }

    void Update()
    {
        // Controlar animaciones basadas en la entrada del jugador
        HandleAnimations();
    }

    void HandleAnimations()
    {
        // Verificar si el personaje está caminando (basado en la entrada horizontal/vertical)
        bool getWalking = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        animator.SetBool(isWalking, getWalking);

        
        animator.SetBool(isOnGround, playerMovementController.grounded);
    }
    void Step()
    {
        Debug.Log("Took Step");
    }
    

}
