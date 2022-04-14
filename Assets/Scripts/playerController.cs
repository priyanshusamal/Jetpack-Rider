using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float JetForce = 50f;
    public float ForwardSpeed = 3f;
    [SerializeField]
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool fire = Input.GetMouseButton(0);
        if(fire){
            rb.AddForce(new Vector2(0,JetForce));
        }




        Vector2 newVec = rb.velocity;
        newVec.x = ForwardSpeed;
        rb.velocity = newVec;
    }
}
