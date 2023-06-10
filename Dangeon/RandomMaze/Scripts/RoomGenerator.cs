using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class RoomGenerator : MonoBehaviour
{
    // [Header("Status")]
    [SerializeField] private enum Status { roomGenerator, doorGenerator, RoomAsign, Render };
    [SerializeField] private Status status;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private BoxCollider renderBox;

    [Header("Room")]
    [SerializeField] private GameObject parentFloor;
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject door;
    [SerializeField] private List<GameObject> roomList;
    [SerializeField] private List<GameObject> doorList;
    [SerializeField] private List<GameObject> prefabList;

    [Header("Enemies")]
    [SerializeField] private GameObject boss;
    [SerializeField] private List<GameObject> bossList;

    [Header("Stats")]
    [SerializeField] private int level;
    [SerializeField] private float scale;


    // Start is called before the first frame update
    void Start()
    {
        room.transform.localScale = new(scale, 1, scale);
        roomList.Add(Instantiate(room, Vector3.zero, room.transform.rotation));
        roomList[^1].transform.SetParent(parentFloor.transform);
        level--;

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        renderBox = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        switch (status)
        {
            case Status.roomGenerator:

                roomList.Add(roomList[Random.Range(0, roomList.Count - 1)].GetComponent<Room>().CreateRoom(room, scale));
                //roomList[^1].transform.SetParent(parentFloor.transform);
                //roomList[^1].transform.GetChild(0).gameObject.SetActive(false);

                level--;
                if (level <= 0)
                    status++;
                break;
            case Status.doorGenerator:
                foreach (GameObject pattern in roomList)
                {
                    foreach (GameObject child in pattern.GetComponent<Room>().GetChildList())
                    {
                        door.transform.localScale = new(scale, 1, scale);
                        door.transform.position = pattern.transform.position;
                        door.transform.LookAt(child.transform.position);
                        doorList.Add(Instantiate(door));
                        doorList[^1].transform.SetParent(pattern.transform);
                        pattern.GetComponent<Room>().AddDoorToList(doorList[^1]);
                    }
                }

                roomList[0].GetComponent<Room>().VisualFirtsRoom();
                
                status++;
                break;
            case Status.RoomAsign:

                // Asignar el tipo de habitaciones
                GameObject go = Instantiate(prefabList[0]);
                go.transform.SetParent(roomList[^1].transform.GetChild(0));
                go.transform.position = go.transform.parent.transform.position;


                int rN = Random.Range(0, bossList.Count - 1);
                bossList[rN].transform.position = roomList[^1].transform.position;
                boss = Instantiate(bossList[rN]);

                status++;
                break;
            case Status.Render:

                break;
            default:
                break;
        }
        
    }
}
