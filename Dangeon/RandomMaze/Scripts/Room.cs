using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomCollider[] points;
    public enum RoomPosition { Top, Bottom, Right, Left, def };
    [SerializeField] private List<RoomPosition> freeRooms;

    [SerializeField] private List<GameObject> child;
    [SerializeField] private List<GameObject> doorList;
    private RoomPosition rp;

    [field: SerializeField] public List<GameObject> enemiesList { get; private set; }


    #region Crea una nueva habitación
    /// <summary>
    /// Crea una nueva habitación
    /// </summary>
    /// <param name="room">Habitación standar</param>
    /// <param name="scale"></param>
    /// <returns></returns>
    #endregion
    public GameObject CreateRoom(GameObject room, float scale)
    {
        freeRooms.Clear();

        foreach (RoomCollider item in points)
        {
            if (!item.room)
            {
                freeRooms.Add((RoomPosition)item.roomPosition);
            }
        }

        if (freeRooms.Count > 0)
            rp = freeRooms[Random.Range(0, freeRooms.Count - 1)];
        else
            rp = RoomPosition.def;

        switch (rp)
        {
            case RoomPosition.Top:
                child.Add(Instantiate(room, transform.position + (Vector3.forward * scale), transform.rotation));
                child[^1].transform.SetParent(transform);
                return child[^1];
            case RoomPosition.Bottom:
                child.Add(Instantiate(room, transform.position + (Vector3.back * scale), transform.rotation));
                child[^1].transform.SetParent(transform);
                return child[^1];
            case RoomPosition.Right:
                child.Add(Instantiate(room, transform.position + (Vector3.right * scale), transform.rotation));
                child[^1].transform.SetParent(transform);
                return child[^1];
            case RoomPosition.Left:
                child.Add(Instantiate(room, transform.position + (Vector3.left * scale), transform.rotation));
                child[^1].transform.SetParent(transform);
                return child[^1];
            default:
                return child[^1].GetComponent<Room>().CreateRoom(room, scale);
        }
    }
    public void AddDoorToList(GameObject door)
    {
        doorList.Add(door);
    }
    #region Asigna el tipo de habitación
    /// <summary>
    /// Assign the kind of room
    /// </summary>
    /// <param name="room">Asigna el nombre de la habitación.</param>
    /// <param name="enemy">Asigna un enemigo de la lista.</param>
    #endregion
    public void AssignRoom(GameObject room, GameObject enemy)
    {
        GameObject go = Instantiate(room);
        go.transform.SetParent(transform.GetChild(0));
        go.transform.position = go.transform.parent.transform.position;
        Destroy(transform.GetChild(0).transform.GetChild(0).gameObject);

        go = transform.GetChild(0).transform.GetChild(1).gameObject;

        for (int i = 0; i < go.transform.GetChild(0).transform.childCount; i++)
        {
            enemy.transform.position = go.transform.GetChild(0).transform.GetChild(i).transform.position;
            enemiesList.Add(Instantiate(enemy));
            enemiesList[^1].transform.SetParent(transform.GetChild(3));
        }

        //enemy.transform.position = transform.position;
        //enemiesList.Add(Instantiate(enemy));
        //enemiesList[^1].transform.SetParent(transform.GetChild(3));
    }
    public void VisualFirtsRoom()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        foreach (GameObject item in doorList)
        {
            item.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public List<GameObject> GetChildList()
    {
        return child;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemiesList)
            {
                enemy.GetComponent<Enemy>().PlayerInRoom();
            }

            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);

            foreach (GameObject item in doorList)
            {
                item.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemiesList)
            {
                enemy.GetComponent<Enemy>().PlayerInRoom();
            }
        }
    }
}
