using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallex : MonoBehaviour
{

    public GameObject cam;
    private float length, startpos;
    public float parallexEffect;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        // length = GetComponent<SpriteRenderer>().bounds.size.x;
        length = 12f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    { 
        float temp =  (cam.transform.position.x * (1 - parallexEffect));
        float dist = (cam.transform.position.x * parallexEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z); 
        if(temp > startpos + length) startpos += length;
        else if(temp < startpos - length) startpos -= length;
    }
}
