using UnityEditor;
using UnityEngine;

public class Action
{
    private CharacterController characterController;
    private GameObject gameObject;
    private Animator animator;

    private Vector3 playerVelocity;
    private Vector3 move;


    #region Contructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="characterController"></param>
    /// <param name="gameObject"></param>
    #endregion
    public Action (CharacterController characterController, GameObject gameObject, Animator animator)
    {
        this.characterController = characterController;
        this.gameObject = gameObject;
        this.animator = animator;
    }

    #region Gravity
    /// <summary>
    /// Gravedad aplicada al personaje
    /// </summary>
    #endregion
    public void Gravity()
    {
        // Se aplica la gravedad
        playerVelocity.y = characterController.isGrounded ? -9.8f * Time.deltaTime : playerVelocity.y - 9.8f * Time.deltaTime;

        characterController.Move(playerVelocity);
    }

    #region Run
    /// <summary>
    /// Acción de Correr
    /// </summary>
    /// <param name="movingSpeed"></param>
    #endregion
    public void Run(float movingSpeed)
    {
        // Mueve al jugador segun los botones

        move = Input.GetAxis("Horizontal") * Camera.main.transform.right +
               Input.GetAxis("Vertical") * Camera.main.transform.forward;
        move.y = 0;
        move = Vector3.ClampMagnitude(move, 1);
        characterController.Move(move * movingSpeed * Time.deltaTime);

        // Rota segun las direccion de movimiento
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;

            // Animación de correr
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

    }

    #region Attack
    /// <summary>
    /// Ataque básico del personaje
    /// </summary>
    /// <param name="attackSpeed">Velocidad de attaque del objeto 'stats'</param>
    #endregion
    public void Attack(float attackSpeed)
    {
        animator.SetBool("Attack", true);

        animator.SetFloat("attackSpeed", attackSpeed);
    }
}
