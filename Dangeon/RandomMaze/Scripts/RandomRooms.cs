using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRooms : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private GameObject room;
    [SerializeField] private List<GameObject> roomList;

    [Header("Currents")]
    [SerializeField] private float currentTime;

    [Header("Level")]
    [SerializeField] private int level = 3;

    // Start is called before the first frame update
    void Start()
    {
        GameObject r = room;
        roomList.Add(Instantiate(r, new Vector3(50, 0, 50), room.transform.rotation));
        level--;
    }

    private void Update()
    {
        if (level > 0)
        {
            if (currentTime > 0.1f)
            {
                GameObject r = room;
                roomList.Add(roomList[Random.Range(0, roomList.Count - 1)].GetComponent<Room>().CreateRoom(r));
                //Random.Range(0, roomList.Count - 1)
                level--;
                currentTime = 0;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
    }
}
