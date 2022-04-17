using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    
    // [SerializeField]
    // GameObject coinPrefab = null;
    [Header("PlayerPos")]
    public Transform groundCheckTransform;
    private bool isGrounded;
    [SerializeField] private LayerMask groundCheckLayerMask;

    [SerializeField]
    private bool isDead = false;
    
    [Header("Health")]

    [SerializeField]
    private int hearts = 3;
    public Image[] health;
    // private int totalNoOfHearts = 3;
    // public Sprite fullHeart,emptyHeart;
    
    public float Fuel = 10f;
    [SerializeField]
    private Slider fuelSlider;
    [SerializeField]
    private Gradient fuelBarColour;
    [SerializeField]
    private Image fill;

    [Header("Physics")]
    private Animator anim;
    public float JetForce = 50f;
    public float ForwardSpeed = 4f;

    [SerializeField]
    private Rigidbody2D rb;



    [Header("Score")]
    [SerializeField]
    private GameObject ScoreBoard;
    [SerializeField]
    private Text Money;
    public int Coins = 0;
    public int highScore;

    [SerializeField]
    private Text HighestScore;
    [SerializeField]
    private Text TotalScore;

    [Header("Audio")]
    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;
    public AudioClip coinCollectAudio;
    public AudioClip damageAudio;
    public AudioClip deathAudio;

    [Header("Effects")]
    [SerializeField]
    private GameObject Shock;
    [SerializeField] private ParticleSystem jetFire;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fill.color = fuelBarColour.Evaluate(1f);
        UpdateHealthBar();
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool fire = Input.GetMouseButton(0);
        if(fire && !isDead && Fuel > 0)
        {
            Fuel -= 2*Time.deltaTime;
            rb.AddForce(new Vector2(0,JetForce));
            anim.SetBool("fly",true);
        }
        else if(!isDead)
        {
            if(isGrounded && Fuel <= 10)
            {
                Fuel += 2*Time.deltaTime;
            }
            anim.SetBool("fly",false);
        }



        if(!isDead)
        {
            Vector2 newVec = rb.velocity;
            newVec.x = ForwardSpeed;
            rb.velocity = newVec;
        }
         
        if(isDead && isGrounded){
            // UpdateScores(TotalScore,HighestScore);
            TotalScore.text = Coins.ToString();
            ScoreBoard.SetActive(true);
            if(Coins>highScore)
            {
                highScore = Coins;
                PlayerPrefs.SetInt("HighScore",Coins);
            }

            HighestScore.text = highScore.ToString();
            // RestartGame();
        }
        Money.text = Coins.ToString();
        UpdateGroundStatus();
        UpdateFuelStatus();         
        AdjustJetPack(fire);
        AdjustFootstepsAndJetpackSound(fire);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Coin")){
            AddCoins(collider);
        }
        else if(collider.gameObject.CompareTag("lazer") && !isDead){
            HitByLazer(collider);
        }
        else if(collider.gameObject.CompareTag("Collector") && !isDead){
            Coins = 0;
            anim.SetTrigger("Damage");
            collider.gameObject.GetComponent<AudioSource>().Play();
        }
    }
    void HitByLazer(Collider2D lazerCollider)
    {
        AudioSource lazerZap = lazerCollider.gameObject.GetComponent<AudioSource>();
            lazerZap.PlayOneShot(damageAudio);
            Shock.GetComponent<Animator>().SetTrigger("shock");
            lazerCollider.gameObject.GetComponent<Collider2D>().enabled = false;            

        if(hearts > 0)
        {
            // if(maxCoins<Coins){maxCoins=Coins;}
            // totalCoins+=Coins;
            anim.SetTrigger("Damage");
            hearts-=1;
        }
        else if(hearts <= 0)
        {
            // lazerZap.PlayOneShot(deathAudio);
            // lazerCollider.enabled = false;            

            isDead  = true;
            anim.SetTrigger("Dead");
            Camera.main.gameObject.GetComponent<AudioSource>().Stop();
            // lazerCollider.gameObject.GetComponent<lazer>().enabled = false;            
        }
        UpdateHealthBar();
    }

    #region Adjust

    void AddCoins(Collider2D coinCollider)
    {
        // AudioSource audio = coinCollider.gameObject.GetComponent<AudioSource>();
        // audio.Play();
        // coinPrefab = coinCollider.gameObject;
        AudioSource.PlayClipAtPoint(coinCollectAudio, transform.position);
        Destroy(coinCollider.gameObject);
        Coins += 1;

    }
    
    void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !isDead && isGrounded;
        jetpackAudio.enabled = !isDead && !isGrounded;
        if (jetpackActive)
        {
            jetpackAudio.volume = 1.0f;
        }
        else
        {
            jetpackAudio.volume = 0.5f;
        }
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
        
            jetpackEmission.rateOverTime = 10f;
        }
    }
    #endregion
     
    #region  Update

    
    void UpdateHealthBar()
    {
        for(int i = 0; i < health.Length; i++){
            if(i<hearts){
                health[i].enabled = true;
            }
            else{
                health[i].enabled = false;
            }
        }
    }
    public void UpdateFuelStatus()
    {
        fuelSlider.value = Fuel;
        fill.color = fuelBarColour.Evaluate(fuelSlider.normalizedValue);
    }

    void UpdateGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position,0.1f,groundCheckLayerMask);
        anim.SetBool("isGrounded",isGrounded);
    }

    #endregion

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
}

