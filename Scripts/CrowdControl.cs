using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    public bool canAttack { get; private set; } = true;
    public bool canHability { get; private set; } = true;


    private float currentTimeStun = 0;

    public bool stun { get; private set; } = false;
    public bool slow { get; private set; } = false;

    private void Update()
    {
        if (stun)
        {
            canMove = false;
            TimerStun();
        } else
        {
            canMove = true;
        }
    }

    /* * * * * * *
     * T I M E R *
     * * * * * * */
    private void TimerStun ()
    {
        if (currentTimeStun < 1)
        {
            currentTimeStun += Time.deltaTime;
        }
        else
        {
            stun = false;
            currentTimeStun = 0;
        }
    }

    /* * * * * * *
     * R E S E T *
     * C R O W D *
     * * * * * * */
    public void SetStun ()
    {
        stun = true;
        currentTimeStun = 0;
    }

    public void SetSlow ()
    {
        slow = true;
    }

}
