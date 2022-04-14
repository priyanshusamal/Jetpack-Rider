using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    
    // [SerializeField]
    // GameObject coinPrefab = null;
    [SerializeField]
    private bool isDead = false;
    [SerializeField]
    private int Coins = 0;
    public float JetForce = 50f;
    public float ForwardSpeed = 4f;
    [SerializeField]
    private Rigidbody2D rb;
    private Animator anim;



    public Transform groundCheckTransform;
    private bool isGrounded;
    [SerializeField] private LayerMask groundCheckLayerMask;
    [SerializeField] private ParticleSystem jetFire;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void UpdateGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position,0.1f,groundCheckLayerMask);
        anim.SetBool("isGrounded",isGrounded);
    }
    void AdjustJetPack(bool jetPackActive){
        var jetpackEmission = jetFire.emission;
        jetpackEmission.enabled = !isGrounded;
        if(jetPackActive)
        {
            jetpackEmission.rateOverTime = 300f;
        }
        else
        {
        
            jetpackEmission.rateOverTime = 70f;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        bool fire = Input.GetMouseButton(0);
        if(fire && !isDead){
            rb.AddForce(new Vector2(0,JetForce));
            anim.SetBool("fly",true);
        }
        else if(!isDead){anim.SetBool("fly",false);}



        if(!isDead)
        {
            Vector2 newVec = rb.velocity;
            newVec.x = ForwardSpeed;
            rb.velocity = newVec;
        }
        
        UpdateGroundStatus();
        AdjustJetPack(fire);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Coin")){
            CollectCoin(collider);
        }
        else{
            HitByLazer(collider);
        }
    }
    void HitByLazer(Collider2D lazerCollider)
    {
        if(Coins > 0)
        {
            anim.SetTrigger("Damage");
            Coins = 0;
        }
        else if(Coins <= 0)
        {
            isDead  = true;
            anim.SetTrigger("Dead");
        }
    }

    void CollectCoin(Collider2D coinCollider)
    {
        Coins += 1;
        // coinPrefab = coinCollider.gameObject;
        Destroy(coinCollider.gameObject);
    }
}
