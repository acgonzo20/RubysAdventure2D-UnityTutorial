using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Accessed the RubyController component on the GameObject
        RubyController controller = other.GetComponent<RubyController>();

        //Check to see that the controller triggering the trigger is infact Ruby (not an enemy, or else game would break)
        if(controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                
                //Funtion which adds one health to RubyController
                controller.ChangeHealth(1);
                //Destroy is built in Unity function, destorys game object that current script is attached to (so the health pack)
                Destroy(gameObject);

                //Play the sound of the clip
                controller.PlaySound(collectedClip);
            }
        }

    }
}
