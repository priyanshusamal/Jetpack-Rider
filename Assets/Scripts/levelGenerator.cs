using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;
    // Start is called before the first frame update
    void Start()
    {
        float height = 2f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }
    void AddLevel(float lastRoomPosX){
        int randomRoomIndex = Random.Range(0,availableRooms.Length);
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = lastRoomPosX + roomWidth * 0.5f;
        room.transform.position = new Vector3(roomCenter,0,0);
        currentRooms.Add(room);
    }
    private void GeneratelevelIfRequired(){
        List<GameObject> roomsToRemove = new List<GameObject>();

        bool addRooms = true;
        float playerX = transform.position.x;
        float removeRoomX = playerX - screenWidthInPoints; //point/postition after which object shoud be removed
        float addRoomX = playerX + screenWidthInPoints; // point/position after which an object should be instantiate 
        float lastRoomPosX = 0;

        foreach(var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = (float)room.transform.position.x - (roomWidth * 0.5);
            float roomEndX = rooomStartX + roomWidth;
            if(roomStartX > addRoomX){
                addRooms = false;
            }
            if(roomEndX < removeRoomX){
                roomsToRemove.Add(room);
            }
            lastRoomPosX = Mathf.Max(lastRoomPosX,roomEndX);
        }
        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }
        if(addRooms){
            addRooms(lastRoomPosX);
        }
    }
    // Update is called once per frame
    private IEnumerator GeneratorCheck()
    {
        while(true)
        {
            GeneratelevelIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
