using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomCollider[] points;
    public enum RoomPosition { Top, Bottom, Right, Left, def };
    [SerializeField] private List<RoomPosition> freeRooms;

    [SerializeField] public List<GameObject> child;
    private RoomPosition rp;


    public GameObject CreateRoom(GameObject room, float scale)
    {
        ClearList();

        if (freeRooms.Count > 0)
            rp = freeRooms[Random.Range(0, freeRooms.Count - 1)];
        else
            rp = RoomPosition.def;

        switch (rp)
        {
            case RoomPosition.Top:
                child.Add(Instantiate(room, transform.position + (Vector3.forward * scale), transform.rotation));
                return child[^1];
            case RoomPosition.Bottom:
                child.Add(Instantiate(room, transform.position + (Vector3.back * scale), transform.rotation));
                return child[^1];
            case RoomPosition.Right:
                child.Add(Instantiate(room, transform.position + (Vector3.right * scale), transform.rotation));
                return child[^1];
            case RoomPosition.Left:
                child.Add(Instantiate(room, transform.position + (Vector3.left * scale), transform.rotation));
                return child[^1];
            default:
                return child[^1].GetComponent<Room>().CreateRoom(room, scale);
        }
    }

    private void ClearList ()
    {
        freeRooms.Clear();

        foreach (RoomCollider item in points)
        {
            if (!item.room)
            {
                freeRooms.Add((RoomPosition)item.roomPosition);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            //transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
