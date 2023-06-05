using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] initialRooms;

    public List<GameObject> rooms;

    private void Start()
    {
        Invoke("Spawn", 0.1f);
    }

    private void Spawn()
    {
        GameObject randomRoom = RandomRoom(initialRooms);
        Instantiate(randomRoom, transform.position, randomRoom.transform.rotation);
    }

    private GameObject RandomRoom(GameObject[] go)
    {
        return go[Random.Range(0, go.Length)];
    }
}
