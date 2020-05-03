using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    //public allows you to change/access outside of the script (in editor or other script)
    public int maxHealth = 5;

    //This first statement is a property which allows you to only access currentHealth in certain functions (get whatever is in the second box, which is returning currentHealth)
    public int health { get { return currentHealth; }}
    int currentHealth;

    //Try to expose a new variable to change the speed of the character (public)
    public float speed = 3.0f;

    //Creates a Rigidbody2D variable to help with collisions and physics
    Rigidbody2D rigidbody2d;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    //Variables used to not instantly kill player when inside of the damage zone
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public GameObject projectilePrefab;

    AudioSource audioSource;
    public AudioClip throwCog;

    // Start is called before the first frame update
    void Start()
    {
        //Give the rigidbody of the gameobject that script is attatched to
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //When game starts characters health is full (maybe not what we want in save)
        currentHealth = maxHealth;

        //For getting consistent frame rates, 60 is what Nintendo Switch and most consoles have
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        audioSource = GetComponent<AudioSource>();
    }

    //Play sound through Ruby because object would be destroyed otherwise
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, 1.0f);
    }


    // Update is called once per frame
    void Update()
    {
        
        //Create variables which are the values(-1 and 1) received between input (controller or keyboard)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //Check to see that move x or y is not equal to zero, then set ruby looking in the appropriate direction
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        //Get the correct direction for the animator
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //Variable which contains values of x,y and z
        Vector2 position = rigidbody2d.position;

        //Value which changes position on press of key/stick movement (Time.deltaTime = time it takes for frame to be rendered)
        position = position + move * speed * Time.deltaTime;
        //Will move character to where you want, but will stop the jittering of gameobject against gameobject
        rigidbody2d.MovePosition(position);

        //Removed Time.deltaTime, essentially creates a timer, and when timer is finished it sets her invincibility back to false
        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }


        //Check to see if you pressed the X key, "talk" button
        if(Input.GetKeyDown(KeyCode.X))
        {
            //There are multiple was to check raycast
            //This one creates a raycast that  is slightly higher than the collision box which is at Ruby's feet.
            //It then checks which way Ruby is looking
            //Then the maxium reach distance is 1.5f units
            //Finally it is making sure that the object that it is interacting with in on the NPC layer
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                if(hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if(character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }

    }

    //A function to either add or subtract and integer for player's health
    public void ChangeHealth(int amount)
    {

        //Check to see if player is currently standing inside of the damage zone, turns her invincible for short amount of time which is updated above
        if (amount < 0)
        {
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        //Mathf.Clamp is a function to ensure the first parameter never goes below the second or above the third
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        
    }

    void Launch()
    {
        //Instantiate is a Unity Function which takes an objects as the first parameter and creates a cope at the position in the second parameter, with rotation in the third parameter
        //Quanternion.identity means no rotation 
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        //Getting projectile script, and call launch
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        //Make animator play launching motion
        animator.SetTrigger("Launch");

        //Play launch sound
        audioSource.PlayOneShot(throwCog);
      
    }

 

}
