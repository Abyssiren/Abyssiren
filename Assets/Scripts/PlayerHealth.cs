using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth = 1000;
    public float currHealth;
    public float damageChunk = 50;

    //recover if you don't take damage. Hurt and invincible when you do. Transition to stop from hurt, then to recover after a bit.
    public enum HealthState
    {
        Recover,
        Stop,
        Hurt
    }

    public HealthState state;
    //damage invincibilty time
    public float hurtInvinTime = 2;
    //how long from stop to recover
    public float recoverTime = 5;
    public float recoverRate = 5;

    public float timer = 0;
    // Use this for initialization
    void Start () {
        state = HealthState.Recover;
        currHealth = maxHealth;
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("YOU AND I COLLIDE");
        if (collider.gameObject.tag == "Fish" && state != HealthState.Hurt)
        {
            //Debug.Log("got hurt" + gameObject.name);
            GameObject fish = collider.gameObject;
            //take the d
            currHealth = currHealth - damageChunk;
            state = HealthState.Hurt;
            timer = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        timer += Time.deltaTime;
        switch (state)
        {
            case HealthState.Recover:
                currHealth += recoverRate;
                if (currHealth > maxHealth)
                    currHealth = maxHealth;
                break;

            case HealthState.Stop:
                if(timer >= recoverTime)
                {
                    state = HealthState.Recover;
                    timer = 0;
                }
                break;

            case HealthState.Hurt:
                if(timer >= hurtInvinTime)
                {
                    state = HealthState.Stop;
                    timer = 0;
                }
                break;
        }
		
	}
}
