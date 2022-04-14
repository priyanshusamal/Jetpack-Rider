using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject targetPlayer;
    [SerializeField]
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position.x - targetPlayer.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float posX = targetPlayer.transform.position.x;
        Vector3 newPos = transform.position;
        newPos.x = posX + distance;
        transform.position = newPos;
    }
}
