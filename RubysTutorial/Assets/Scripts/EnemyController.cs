using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Speed the enemy is moving
    public float speed = 3.0f;
    //If true, character will move in the Y direction
    public bool vertical;
    //The seconds it takes for the direction of the enemy to change
    public float changeTime = 3.0f;
    //Robots start out broken
    bool broken = true;

    public ParticleSystem smokeEffect;
    Rigidbody2D rigidbody2D;
    Animator animator;

    

    //Timer and direction of enemy
    float timer;
    int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Set timer the amount of changeTime
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Test to see if the robot is broken (broken in terms of the game, not as an object)
        if (!broken)
        {
            return;
        }

        //Decrement the timer from that of game time
        timer -= Time.deltaTime;

        //If timer is less less than 0, switch direction and add time
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }


        Vector2 position = rigidbody2D.position;

        //If vertical is true go up and down, else go left and right
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    //On collision take health from player
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    //when hit with the cog, the robots are reparied
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;

        //do the fixed animation
        animator.SetTrigger("Fixed");

        smokeEffect.Stop();
    }

}
