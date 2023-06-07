using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController_Script : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] protected Transform target;        // El jugador que el enemigo seguirá
    [SerializeField] private float gravityValue = -39.24f;
    [SerializeField] protected float distance;
    [SerializeField] protected Transform rightHand;
    [SerializeField] protected Transform leftHand;

    [Header("Vectors Aux")]
    protected Vector3 direction;
    protected Vector3 velocitySpeed;

    [Header("Dash")]
    [SerializeField] protected bool activeDash = false;
    [SerializeField] protected int currentDash = 3;
    [SerializeField] protected float maxDistanceDash = 5;
    private float speedDash;
    private float distanceDash;
    private Vector3 directionDash;
    private Vector3 startDash;
    private Vector3 finishDash;

    [Header("Shoot")]
    [SerializeField] protected float sp;
    [SerializeField] protected GameObject arm;
    [SerializeField] protected List<GameObject> armShooting;
    [SerializeField] protected float currentTimeShoot;

    [Header("Controller")]
    [SerializeField] protected CharacterController controller;
    [SerializeField] protected StatsPattern_script stats;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;

    protected virtual void Start()
    {
        armShooting.Add(Instantiate(arm));
        stats = GetComponentInChildren<StatsPattern_script>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Invoke(UpdateAgent(), 0.5f);
    }

    private void UpdateAgent()
    {
        agent.speed = stats.speed;
        agent.stoppingDistance = stats.range;
    }

    protected void ActionFollow()
    {
        if (Vector3.Distance(target.position, transform.position) > stats.range)
        {
            // Calcula la dirección hacia el jugador
            direction = target.position - transform.position;
            direction.y = 0;
            direction = Vector3.ClampMagnitude(direction, 1);

            // Calcula la velocidad de movimiento
            velocitySpeed = stats.speed * Time.deltaTime * direction;

            // Gira el personaje hacia la dirección que se mueve
            if (velocitySpeed != Vector3.zero)
            {
                gameObject.transform.forward = direction;
            }

            velocitySpeed.y = gravityValue * Time.deltaTime;
            // Mueve al enemigo hacia el jugador
            //transform.position += velocidadMovimiento;
            controller.Move(velocitySpeed);
        }
    }
    protected void ActionDash ()
    {
        if (!activeDash)
        {
            startDash = transform.position;
            finishDash = target.position;

            directionDash = finishDash - startDash;
            directionDash.y = 0;
            directionDash = Vector3.ClampMagnitude(directionDash, 1);

            speedDash = 0;
            activeDash = true;
        } else
        {
            velocitySpeed = (stats.speed * speedDash) * Time.deltaTime * directionDash;

            if (velocitySpeed != Vector3.zero)
            {
                gameObject.transform.forward = directionDash;
            }


            velocitySpeed.y = gravityValue * Time.deltaTime;
            controller.Move(velocitySpeed);

            distanceDash = Vector3.Distance(startDash, transform.position) - Vector3.Distance(startDash, finishDash);

            if (distanceDash < maxDistanceDash && speedDash < stats.speed*2)
            {
                speedDash += 0.5f;
            }
            else if (distanceDash > maxDistanceDash && speedDash > 0)
            {
                speedDash -= 2.5f;
            } else if (speedDash <= 0)
            {
                activeDash = false;
                currentDash--;
            }
        }
    }
    protected void ActionShoot()
    {
        //Crear un nuevo objeto a partir del prefab
        if (!armShooting[0].activeSelf)
        {
            armShooting[0].transform.forward = target.position - armShooting[0].transform.position;
            armShooting[0].SetActive(true);
        }
    }

    protected void DriverShoot()
    {
        foreach (var item in armShooting)
        {
            CollisionShoot(item);
            if (item.transform.localScale.y <= 0)
            {
                item.SetActive(false);
                item.transform.position = rightHand.position;
                item.transform.localScale = new(1, 1, 1);
            }
            else
            {
                Vector3 v3 = item.transform.forward;
                v3.y = 0;
                item.GetComponent<Rigidbody>().velocity = stats.speedAttack * Time.deltaTime * v3 * sp;
                item.transform.localScale -= 5 * Time.deltaTime * new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }


    /* * * * * * * * * * *
     * C O L L I S I O N *
     * * * * * * * * * * */

    protected void CollisionShoot (GameObject shoot)
    {
        Ray ray = new(shoot.transform.position, shoot.transform.forward);

        RaycastHit[] hits = Physics.SphereCastAll(ray, 2f);


        if (hits.Length > 0)
        {
            foreach (RaycastHit item in hits)
            {
        
                if (item.collider.CompareTag("Player") &&
                    Vector3.Distance(item.collider.transform.position, shoot.transform.position) < 3)
                {
                    shoot.transform.localScale = new(0, 0, 0);
                    Debug.Log("¡¡OneShoot!!");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (armShooting.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(armShooting[0].transform.position, 2f);//(armShooting[0].transform.position + armShooting[0].transform.forward));
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != gameObject && activeDash)
        {
            Vector3 v3 = velocitySpeed*10;
            v3.y -= gravityValue;
            PlayerController_script playerController = other.GetComponent<PlayerController_script>();
            playerController.Feedback("stun", stats.damage, v3);
            activeDash = false;
            currentDash = 0;
        }
    }
}
