using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Agents")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float distance;
    [SerializeField] private Stats stats;

    [SerializeField] private bool inRoom;

    private void Start()
    {
        stats = new Stats(3, 0.5f, 20, 1.0f);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        if (inRoom)
        {
            agent.SetDestination(target.position);

            //if (distance < agent.stoppingDistance && Random.value < 0.01f)
            //    TPBeforePlayer();
        }
        else
        {
                agent.SetDestination(transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (stats.healt <= 0)
        {
            Destroy(gameObject);
        }
    }

    #region El jugador esta en la sala
    /// <summary>
    /// Actualiza el estado del jugador en la sala
    /// </summary>
    #endregion
    public void PlayerInRoom()
    {
        inRoom = true;
    }


    //Teletransportarse detras del jugador
    public void TPBeforePlayer ()
    {
        Vector3 forward = target.transform.forward;
        forward.y = 0;
        Vector3 position = target.transform.position;
        position.y = transform.position.y;

        gameObject.transform.position = position - forward * 7;
    }

    #region Impacto de ataque
    /// <summary>
    /// Cuando se colisiona con el area de daño del oponente se pasa al stats para que haga el calculo
    /// </summary>
    /// <param name="impactDamage">Cantidad de daño recibido</param>
    #endregion
    public void ImpactDamage(float impactDamage)
    {
        Debug.Log("Ouch!!");
        stats.ImpactDamage(impactDamage);
    }
}
