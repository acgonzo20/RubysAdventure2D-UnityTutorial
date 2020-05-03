using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        //Accessed the RubyController component on the GameObject
        RubyController controller = other.GetComponent<RubyController>();

        //Check to see that the controller triggering the trigger is infact Ruby (not an enemy, or else game would break)
        if (controller != null)
        {
                //Funtion which subtracts one health to RubyController
                controller.ChangeHealth(-1);
        }

    }
}
