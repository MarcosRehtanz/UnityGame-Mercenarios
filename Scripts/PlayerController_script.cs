using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_script : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CrowdControl crowdControl;
    private CharacterController characterController;
    private Stats stats;
    private Action action;
    private Animator animator;

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
    private Vector3 playerVelocity;
    private bool rolling;
    private bool canMove;

    [Header("Animator")]
    private string animationName;


    private void Start()
    {
        //stats.SetcrowdControl(crowdControl);
        stats = new Stats(20, 1, 20, 1);
        action = new Action(
            gameObject.GetComponent<CharacterController>(),
            gameObject,
            gameObject.GetComponent<Animator>());

        animator = gameObject.GetComponent<Animator>();
        characterController = gameObject.GetComponent<CharacterController>();

        rolling = false;
        canMove = true;
    }

    private void Update()
    {
        action.Gravity();
        Run();
        Attack();
    }

    #region Run
    /// <summary>
    /// Method for to run
    /// </summary>
    #endregion
    protected void Run()
    {
        if (!animator.GetBool("Attack"))
        {
            action.Run(stats.movingSpeed);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
    #region Attack
    /// <summary>
    /// Method for Attack
    /// </summary>
    #endregion
    protected void Attack()
    {
        if (Input.GetKeyDown(keyAttack))
        {
            EnemyDirection();
            ImpactCollision();
            action.Attack(stats.attackSpeed+1.5f);
        }
    }

    #region Enemy Detection Collider
    /// <summary>
    /// Se direcciona hacia donde esta el enemigo mas cercano dentro de un area
    /// </summary>
    #endregion
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
    #region Damage Collider
    /// <summary>
    /// CollisionArm is the Arm Area
    /// </summary>
    #endregion
    private void ImpactCollision()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        forward.y = transform.position.y;

        // Crear un raycast hacia adelante desde la posición y dirección deseada
        Ray ray = new(pos, forward);

        // Realizar el raycast contra el Mesh Collider
        RaycastHit[] hit = Physics.SphereCastAll(ray, areaDamage);

        // Verificar si el raycast colisionó con algo
        if ((hit.Length > 0))
        {
            foreach (RaycastHit item in hit)
            {
                if (item.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = item.collider.GetComponentInChildren<Enemy>();
                    enemy.ImpactDamage(stats.damage);
                }
            }
        }

    }
    
    private void OnDrawGizmos()
    {
        try
        {
            if (animator.GetBool("Attack"))
            {
                Vector3 pos = transform.position;
                Vector3 forward = transform.forward;
                forward.y = transform.position.y;
                Vector3 v3 = pos + forward * areaDamage;

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(v3, areaDamage);

            }
        }
        catch (System.Exception) {}

        // Area to attack
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, areaDetection);
    }

    /* * * * * * * *
     * A C T I O N *
     * P L A Y E R *
     * * * * * * * */


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
            characterController.Move(rollForce * stats.movingSpeed * Time.deltaTime * transform.forward);
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
        characterController.Move(damage * Time.deltaTime * v3);
        stats.ImpactDamage(damage);
        CrowdControl(typeOfControl);
    }
    public void Feedback (float damage, Vector3 v3)
    {
        characterController.Move(damage * Time.deltaTime * v3);
        stats.ImpactDamage(damage);
    }
}
