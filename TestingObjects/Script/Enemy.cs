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


    [SerializeField] private bool inRoom;

    private void Start()
    {
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
}
