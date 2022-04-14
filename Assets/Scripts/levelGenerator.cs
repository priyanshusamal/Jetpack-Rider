using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    public GameObject[] availableObjects;
    public List<GameObject> objects;
    
    public float objMinDistance = 5f;
    public float objMaxDistance = 10f;

    public float objMinY = -2f;
    public float objMaxY = 2f;

    public float objMinRotation = -45f;
    public float objMaxRotation = 45f;
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
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = (float)(room.transform.position.x - (roomWidth * 0.5));
            float roomEndX = roomStartX + roomWidth;
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
            // AddRooms(lastRoomPosX);
            AddLevel(lastRoomPosX);
        }
    }
    // Update is called once per frame
    private IEnumerator GeneratorCheck()
    {
        while(true)
        {
            GeneratelevelIfRequired();
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }

    void AddObject(float lastObjectX){
        int randomIndex = Random.Range(0,availableObjects.Length);
        GameObject obj = (GameObject) Instantiate(availableObjects[randomIndex]);
        float objectPositionX = lastObjectX + Random.Range(objMinDistance,objMaxDistance);
        float randomY = Random.Range(objMinY,objMaxY);
        obj.transform.position = new Vector3(objectPositionX,randomY,0);
        float rotation = Random.Range(objMinRotation,objMaxRotation);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward*rotation);
        objects.Add(obj);
    }
    void GenerateObjectsIfRequired()
    {
        //1
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;
        //2
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;
            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);
            //5
            if (objX < removeObjectsX) 
            {           
                objectsToRemove.Add(obj);
            }
        }
        //6
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }
        //7
        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }
}
