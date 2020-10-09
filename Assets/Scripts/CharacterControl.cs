using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterControl : MonoBehaviour
{

    public Sprite[] idleAnim;
    public Sprite[] jumpingAnim;
    public Sprite[] walkingAnim;
    public Text lifeText;
    public Image endGameScreen;
    public int life = 5;

    int idleAnimCounter;
    int jumpingAnimCounter;
    int walkingAnimCounter;

    SpriteRenderer spriteRenderer;

    Rigidbody2D physics;

    Vector3 vec;
    Vector3 cameraFirstPos;
    Vector3 cameraLastPos;

    float horizontal = 0;
    float idleAnimDelay;
    float jumpingAnimDelay;
    float walkingAnimDelay;


    float endGameScreenCounter = 0;
    float backToMainMenuTime=0;
    bool oneTimeJump = true;

    GameObject cameraobject;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        physics = GetComponent<Rigidbody2D>();
        cameraobject = GameObject.FindGameObjectWithTag("MainCamera");
        cameraFirstPos = cameraobject.transform.position - transform.position;
        //lifeText.text = "LIFE : " + life;
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (oneTimeJump)
            {
                physics.AddForce(new Vector2(0, 325));
                oneTimeJump = false;
            }

        }
    }
    void FixedUpdate()
    {
        Animation();
        characterMove();
        if (life<=0)
        {
            Time.timeScale = 0.4f;
            lifeText.enabled = false;
            endGameScreenCounter += 0.03f;
            endGameScreen.color = new Color(0,0,0, endGameScreenCounter);
            backToMainMenuTime += Time.deltaTime;
            if (backToMainMenuTime>1)
            {
                SceneManager.LoadScene("mainmenu");
            }
        }
    }

    private void LateUpdate()
    {
        cameraControl();
    }
    void characterMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vec = new Vector3(horizontal * 5, physics.velocity.y, 0);
        physics.velocity = vec;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        oneTimeJump = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spell")
        {
            life--;
            lifeText.text = "LIFE  :  " + life;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            life-=5;
            lifeText.text = "LIFE  :  " + life;
        }
        if (collision.gameObject.tag == "EnemyWizard")
        {
            life -= 5;
            lifeText.text = "LIFE  :  " + life;
        }
        if (collision.gameObject.tag == "StageCompleted")
        {
            SceneManager.LoadScene("mainmenu");
        }
    }
    void cameraControl()
    {
        cameraLastPos = cameraFirstPos + transform.position;
        cameraobject.transform.position = Vector3.Lerp(cameraobject.transform.position,cameraLastPos,0.1f);
    }
    void Animation()
    {
        if (oneTimeJump)
        {
            if (horizontal == 0)   //character idle
            {
                idleAnimDelay += Time.deltaTime;
                if (idleAnimDelay > 0.05f)
                {
                    spriteRenderer.sprite = idleAnim[idleAnimCounter++];
                    if (idleAnimCounter == idleAnim.Length)
                    {
                        idleAnimCounter = 0;
                    }
                    idleAnimDelay = 0;
                }
                
            }
            else if (horizontal > 0)    //running positive dir
            {
                walkingAnimDelay += Time.deltaTime;
                if (walkingAnimDelay > 0.05f)
                {
                    spriteRenderer.sprite = walkingAnim[walkingAnimCounter++];
                    if (walkingAnimCounter == walkingAnim.Length)
                    {
                        walkingAnimCounter = 0;
                    }
                    walkingAnimDelay = 0;
                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontal < 0)        //running negative dir
            {
                walkingAnimDelay += Time.deltaTime;
                if (walkingAnimDelay > 0.05f)
                {
                    spriteRenderer.sprite = walkingAnim[walkingAnimCounter++];
                    if (walkingAnimCounter == walkingAnim.Length)
                    {
                        walkingAnimCounter = 0;
                    }
                    walkingAnimDelay = 0;
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
           
        }
        else
        {
            if (physics.velocity.y > 0) //jumping
            {
                spriteRenderer.sprite = jumpingAnim[2];
                
            }
            else if (physics.velocity.y < 0)//after jump downing
            {
                spriteRenderer.sprite = jumpingAnim[7];
                
            }
        }

    }
}
