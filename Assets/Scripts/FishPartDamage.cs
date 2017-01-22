using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPartDamage : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Debug.Log("help me" + gameObject.name);
            //could probably get the color and damage scale in here, too
            transform.parent.GetComponent<FishHealth>().TakeDamage(collision);
        }
        if (collision.gameObject.tag == "Hand")
        {
            Debug.Log("help me" + gameObject.name);
            //could probably get the color and damage scale in here, too
            transform.parent.GetComponent<FishHealth>().TakeStun(collision);
        }
    }

    //when your joint breaks, your parent dies too
    void OnJointBreak(float breakForce)
    {
        Debug.Log("A joint has just been broken!, force: " + breakForce);
        transform.parent.GetComponent<FishHealth>().Broken();
    }

}
