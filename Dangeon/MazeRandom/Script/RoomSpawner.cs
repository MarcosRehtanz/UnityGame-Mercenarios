using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum OpenSide {Top, Right, Bottom, Left }
    public OpenSide openside;

    private RoomTemplates templates;
    private GameObject randomRoom;
    private bool spawned = false;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    public void Spawn ()
    {
        if (!spawned)
        {
            switch (openside)
            {
                case OpenSide.Top:
                    // Need Top door
                    randomRoom = RandomRoom(templates.topRooms);
                    Instantiate(randomRoom, transform.position, randomRoom.transform.rotation);
                    break;
                case OpenSide.Right:
                    // Need Right door
                    randomRoom = RandomRoom(templates.rightRooms);
                    Instantiate(randomRoom, transform.position, randomRoom.transform.rotation);
                    break;
                case OpenSide.Bottom:
                    // Need Bottom door
                    randomRoom = RandomRoom(templates.bottomRooms);
                    Instantiate(randomRoom, transform.position, randomRoom.transform.rotation);
                    break;
                case OpenSide.Left:
                    // Need Left door
                    randomRoom = RandomRoom(templates.leftRooms);
                    Instantiate(randomRoom, transform.position, randomRoom.transform.rotation);
                    break;
                default:
                    break;
            }

            spawned = true;
        }
        
    }

    private GameObject RandomRoom(GameObject[] go)
    {
        return go[Random.Range(0, go.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
        }
    }
}
