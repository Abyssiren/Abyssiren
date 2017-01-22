using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public float maxHealth = 1000;
    public float currHealth;
    public float damageChunk = 50;
	public Image dmgindicator;
    //recover if you don't take damage. Hurt and invincible when you do. Transition to stop from hurt, then to recover after a bit.
    public enum HealthState
    {
        Recover,
        Stop,
        Hurt
    }
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public HealthState state;
    //damage invincibilty time
    public float hurtInvinTime = 2;
    //how long from stop to recover
    public float recoverTime = 5;
    public float recoverRate = 1;

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
            AudioSource audio = this.gameObject.GetComponent<AudioSource>();
            if (audio == null)
            {
                audio = this.gameObject.AddComponent<AudioSource>();
            }
            audio.Stop();
            
            //Debug.Log("got hurt" + gameObject.name);
            GameObject fish = collider.gameObject;
            //take the d
            currHealth = currHealth - damageChunk;
			dmgindicator.color = new Color (dmgindicator.color.r, dmgindicator.color.g, dmgindicator.color.b, 0.6f);
            state = HealthState.Hurt;
            timer = 0;

			if (currHealth <= 0f) {
                audio.PlayOneShot(this.deathSound);
                SceneManager.LoadScene (0);
			} else {
                audio.PlayOneShot(this.hurtSound);
            }

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
