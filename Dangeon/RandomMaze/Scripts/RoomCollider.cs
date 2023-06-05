using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{
    public bool room;
    public enum RoomPosition { Top, Bottom, Right, Left, def };
    public RoomPosition roomPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public RoomPosition FreeRoom()
    {
        return roomPosition;
    }

    public bool GetRoom()
    {
        return room;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
            room = true;
        else
            room = false;
    }
}
