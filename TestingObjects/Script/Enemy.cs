using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Agents")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private bool inRoom;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRoom)
        {
                agent.SetDestination(target.position);
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
        inRoom = !inRoom;
    }
}
