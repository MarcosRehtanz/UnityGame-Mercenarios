using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{

    [Header("Stats")]
    //private int seed;
    [SerializeField] private float exp;
    [SerializeField] private int level;
    [SerializeField] private float scale;

    private enum Status { roomGenerator, doorGenerator, RoomAsign, Render };
    private Status status;
    private Vector3 playerPosition;

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
    [SerializeField] private List<GameObject> enemiesList;

    // Start is called before the first frame update
    void Start()
    {
        room.transform.localScale = new(scale, 1, scale);
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Update()
    {
        switch (status)
        {
            case Status.roomGenerator:

                // Si la lista de las salas estan vacias se crea la sala inicial,
                // luego comienza un ciclo recursivo con el resto de las salas
                if (roomList.Count > 0)
                {
                    int num = Mathf.FloorToInt(Random.value * (roomList.Count - 1));
                    roomList.Add(roomList[num].GetComponent<Room>().CreateRoom(room, scale));
                    //roomList[^1].transform.SetParent(parentFloor.transform);
                    //roomList[^1].transform.GetChild(0).gameObject.SetActive(false);

                    level--;
                    if (level <= 0)
                    {
                        status++;
                    }
                } else
                {
                    roomList.Add(Instantiate(room, Vector3.zero, room.transform.rotation));
                    roomList[^1].transform.SetParent(parentFloor.transform);
                    level--;
                }

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

                /* * * * * *
                 * B O S S *
                 * * * * * */
                GameObject go = Instantiate(prefabList[0]);
                go.transform.SetParent(roomList[^1].transform.GetChild(0));
                go.transform.position = go.transform.parent.transform.position;
                //Debug.Log(roomList[^1].transform.GetChild(0).transform.GetChild(0).gameObject.name);
                Destroy(roomList[^1].transform.GetChild(0).transform.GetChild(0).gameObject);

                int rN = Random.Range(0, bossList.Count - 1);
                bossList[rN].transform.position = roomList[^1].transform.position;
                boss = Instantiate(bossList[rN]);

                /* * * * * * * *
                 * T E S O U R *
                 * * * * * * * */

                /* * * * * * * * *
                 * E N E M I E S *
                 * * * * * * * * */
                for (int i = 0; i < roomList.Count-2; i++)
                {
                    rN = Random.Range(0, enemiesList.Count - 1);
                    int rPf = Mathf.FloorToInt(Random.value * (prefabList.Count-1)) + 1;
                    roomList[i].GetComponent<Room>().AssignRoom(prefabList[rPf], enemiesList[rN]);
                }

                status++;
                break;
            case Status.Render:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            default:
                break;
        }
        
    }
}
