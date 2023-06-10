using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_script : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private StatsPattern_script stats;
    [SerializeField] private CrowdControl crowdControl;
    [SerializeField] private Animator animator;

    [Header("Arm")]
    [SerializeField] private GameObject arm;
    [SerializeField] private MeshCollider armMeshCollider;
    [SerializeField] private float areaDamage;
    [SerializeField] private float areaDetection;

    [Header("Values")]
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f * 5;
    [SerializeField] private float rollForce = 2.25f;
    [SerializeField] private bool isGrouded;
    [SerializeField] private bool statsCanMove;

    [Header("Key Codes")]
    [SerializeField] private KeyCode keyJump;
    [SerializeField] private KeyCode keyAttack;
    [SerializeField] private KeyCode keyRoll;

    [Header("Attributes")]
    private Vector3 move;
    private Vector3 playerVelocity;
    private bool rolling;
    private bool canMove;

    [Header("Animator")]
    private string animationName;
    private float animationTime;


    private void Start()
    {
        stats.SetcrowdControl(crowdControl);
        rolling = false;
        canMove = true;
    }

    private void Update()
    {

        isGrouded = controller.isGrounded;
        GetAnimationTag("Fight");

        if (crowdControl.canMove)
        {
            if (canMove)
            {
                if (Input.GetKeyDown(keyAttack))
                {
                    EnemyDirection();
                    ActionBasicAttack();
                } // ATTACK
                else if (Input.GetKeyDown(keyRoll))
                {
                    AnimationRoll();
                } // ROLL
                else if (animationName != "Fight_FirstPart")
                {
                    ActionMove();
                    ActionJump();
                } //MOVE or JUMP

            }
            else
            {
                bool boolRoll = animator.GetBool("JumpRoll");
                switch (animationName)
                {
                    case "Fight_FirstKick":
                        ActionContinueAttack(2);
                        break;
                    case "Fight_SecondKick":
                        ActionContinueAttack(3);
                        break;
                    case "Fight_ThirdKick":
                        animator.SetInteger("Fight", 0);
                        break;
                    case "JumpRoll":
                        animator.SetBool("JumpRoll", false);
                        break;
                    case "Idle":
                        if (animator.GetInteger("Fight") != 1 && !boolRoll)
                        {
                            canMove = true;
                            rolling = false;
                            armMeshCollider.enabled = false;
                        }
                        break;
                    case "Walk":
                        break;
                    default:
                        canMove = true;
                        armMeshCollider.enabled = false;
                        animator.SetInteger("Fight", 0);
                        break;
                } // When is ATTAKING look the animations to the next attack or roll
            }
        }

        ActionJump();
        if (armMeshCollider.enabled)
            CollisionArm();
        ActionGravity();
        ActionRollMove();
        // Se aplica la velocidad vertical
        controller.Move(playerVelocity * Time.deltaTime);


        animationName = GetAnimationName();
        animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    /* * * * * * * *
     * A C T I O N *
     * P L A Y E R *
     * * * * * * * */

    private void ActionMove ()
    {
        // Mueve al jugador segun los botones
        move = Input.GetAxis("Horizontal") * Camera.main.transform.right +
               Input.GetAxis("Vertical") * Camera.main.transform.forward;
        move.y = 0;
        move = Vector3.ClampMagnitude(move, 1);
        controller.Move(move * stats.GetSpeed() * Time.deltaTime);

        // Rota segun las direccion de movimiento
        if (move != Vector3.zero)
        {
            transform.forward = move;

            // Animación de caminar
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

    }
    private void ActionJump()
    {
        // Resetea la velocidad vertical
        // Limitada por la gravedad minima
        if (isGrouded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        // Changes the height position of the player..
        // Aplica fuerza de salto
        if (Input.GetButtonDown("Jump") && isGrouded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }
    private void ActionGravity()
    {
        // Se aplica la gravedad
        if (isGrouded)
            playerVelocity.y = gravityValue * Time.deltaTime;
        else
            playerVelocity.y += gravityValue * Time.deltaTime;

    }
    private void AnimationRoll()
    {
        canMove = false;
        animator.SetBool("Walk", false);
        animator.SetBool("JumpRoll", true);
        rolling = true;

        
    }
    private void ActionRollMove()
    {
        if (!canMove && rolling)
        {
            controller.Move(rollForce * stats.GetSpeed() * Time.deltaTime * transform.forward);
        }
    }
    private void ActionBasicAttack()
    {
        canMove = false;
        armMeshCollider.enabled = true;
        animator.SetBool("Walk", false);

        animator.SetInteger("Fight", 1);
    }
    private void ActionContinueAttack(int a)
    {
        if (Input.GetKeyDown(keyAttack) && animator.GetInteger("Fight") != a)
        {
            armMeshCollider.enabled = true;
            animator.SetInteger("Fight", a);
        }
        else if (animator.GetInteger("Fight") == a-1)
        {
            animator.SetInteger("Fight", 0);
            armMeshCollider.enabled = false;
        }
    }

    /* * * * * * * * * * * 
     * C O L L I S I O N *
     * * * * * * * * * * */

    private void CollisionArm()
    {
        // Crear un raycast hacia adelante desde la posición y dirección deseada
        Ray ray = new((arm.transform.forward * 3 + arm.transform.position), transform.forward);

        // Realizar el raycast contra el Mesh Collider
        RaycastHit[] hit = Physics.SphereCastAll(ray, areaDamage);

        // Verificar si el raycast colisionó con algo
        if ((hit.Length > 0))
        {
            foreach (RaycastHit item in hit)
            {
                if (item.collider.gameObject.CompareTag("Enemy"))
                {
                    StatsPattern_script stp = item.collider.GetComponentInChildren<StatsPattern_script>();
                    stp.ImpactDamage(stats.damage);
                    armMeshCollider.enabled = false;
                }
            }
        }

    }
    private void EnemyDirection()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit[] raycastHits = Physics.SphereCastAll(ray, areaDetection);

        if (raycastHits.Length > 0)
        {
            foreach (RaycastHit item in raycastHits)
            {
                if (item.collider.CompareTag("Enemy") &&
                    Vector3.Distance(item.collider.transform.position, transform.position) < areaDetection)
                {
                    Vector3 targetPosition = item.collider.transform.position;
                    targetPosition.y = transform.position.y;
                    transform.LookAt(targetPosition);
                    Debug.Log("Objeto detectado: " + item.collider.gameObject.name);
                }
            }
                
        }
    }

    private void OnDrawGizmos()
    {
        // Arm Collider
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((arm.transform.forward * 3 + arm.transform.position), areaDamage);
        
        // Area to attack
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaDetection);
    }

    /* * * * * * * * * * * 
     * A N I M A T I O N *
     * * * * * * * * * * */

    private string GetAnimationName()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            AnimationClip animacionClip = clipInfos[0].clip;
            return animacionClip.name;
        }

        return string.Empty;
    }
    private bool GetAnimationTag(string tag)
    {
        // Obtener el estado actual de la animación
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        // Obtener el tag de la animación actual
        if (stateInfo.IsTag("Fight"))
        {
            return true;
        }

        return false;
    }


    /* * * * * * * * *
     * * C R O W D * *
     * C O N T R O L *
     * * * * * * * * */

    public void CrowdControl (string typeOfControl)
    {
        switch (typeOfControl)
        {
            case "stun":
                crowdControl.SetStun();
                break;
            default:
                break;
        }
    }

    /* * * * * * * * * *
     * F E E D B A C K *
     * * * * * * * * * */
    
    public void Feedback (string typeOfControl, float damage, Vector3 v3)
    {
        controller.Move(damage * Time.deltaTime * v3);
        stats.ImpactDamage(damage);
        CrowdControl(typeOfControl);
    }
    public void Feedback (float damage, Vector3 v3)
    {
        controller.Move(damage * Time.deltaTime * v3);
        stats.ImpactDamage(damage);
    }
}
