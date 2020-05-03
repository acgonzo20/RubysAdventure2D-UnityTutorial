using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;
    // Changed to awake because Unity doesnt run start when you create an object, but on the next frame
    //You want to keep your rigidbody empty
    //Awake is called immediately when object is created
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //When the projectile goes to far it will be destroyed, if not destroyed after hitting something
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if(e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
    }
}
