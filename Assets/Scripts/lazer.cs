using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazer : MonoBehaviour
{

    public Sprite lazerOnSprite;
    public Sprite lazerOffSprite;
    public float toggleInterval = 0.5f; 
    public float rotationSpeed = 0.0f;

    private bool isLazerOn = true;
    private float timeUntilNextToggle;

    private Collider2D lazerCollider;
    private SpriteRenderer lazerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextToggle = toggleInterval;
        lazerCollider = gameObject.GetComponent<Collider2D>();
        lazerRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        timeUntilNextToggle -= Time.deltaTime;

        if(timeUntilNextToggle <= 0)
        {
            isLazerOn = !isLazerOn;
            lazerCollider.enabled = isLazerOn;

            if(isLazerOn)
            {
                lazerRenderer.sprite = lazerOnSprite;
            }
            else
            {
                lazerRenderer.sprite = lazerOffSprite;
            }
            timeUntilNextToggle = toggleInterval;
        }
        transform.RotateAround(transform.position,Vector3.forward,rotationSpeed*Time.deltaTime);
    }
}
