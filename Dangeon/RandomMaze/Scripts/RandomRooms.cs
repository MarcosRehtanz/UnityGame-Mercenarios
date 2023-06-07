using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRooms : MonoBehaviour
{
    // [Header("Status")]
    [SerializeField] private enum Status { roomGenerator, A };
    [SerializeField] private Status status;

    [Header("Room")]
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject room;
    [SerializeField] private List<GameObject> roomList;
    [SerializeField] private List<GameObject> doorList;

    [Header("Stats")]
    [SerializeField] private int level;
    [SerializeField] private float scale;


    // Start is called before the first frame update
    void Start()
    {
        room.transform.localScale = new(scale, 1, scale);
        roomList.Add(Instantiate(room, Vector3.zero, room.transform.rotation));
        level--;
    }

    private void Update()
    {
        if (level > 0)
        {
            roomList.Add(roomList[Random.Range(0, roomList.Count - 1)].GetComponent<Room>().CreateRoom(room, scale));
            level--;
        } else if (level == 0)
        {
            foreach (GameObject pattern in roomList)
            {
                foreach (GameObject child in pattern.GetComponent<Room>().child)
                {
                    door.transform.localScale = new(scale, 1, scale);
                    door.transform.position = pattern.transform.position;
                    door.transform.LookAt(child.transform.position);
                    Instantiate(door);
                }
            }
            level--;
        }
    }
}
