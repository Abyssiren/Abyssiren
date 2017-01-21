using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishHealth : MonoBehaviour {

    public float health = 100;
    private float currHealth;
    private int deathCountdown = 100;
    private Transform cube1;
    private Transform cube2;
    private Transform cube3;
    //death dissolve 
    private Material mat;


    [SerializeField]
    private Transform[] fishParts;

    public float stun;
    public float currStun;
	// Use this for initialization
	void Start () {
        currHealth = health;
        //mat = gameObject.GetComponent<Renderer>().material;

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("help me" + gameObject.name);
        if (collision.gameObject.tag == "Projectile")
        {
            GameObject bullet = collision.gameObject;
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white, 4);
            }

            float hitSpeed = collision.relativeVelocity.magnitude;
            //fun, but pointless. currHealth = currHealth - hitSpeed;

            //take damage and stun.
            //Debug.Log(bullet.GetComponent<TearBeam>());
            currHealth -= bullet.GetComponent<Bullet>().power;
            currStun += stun;
            gameObject.GetComponent<FishEnemyController>().currStun = currStun;
            if (currHealth <= 0)
            {
                //disable AI
                if (deathCountdown == 100)
                {
                    gameObject.GetComponent<FishEnemyController>().enabled = false;
                    gameObject.GetComponent<CharacterJoint>().breakForce = 0.1f;
                    gameObject.GetComponent<Rigidbody>().drag = 0.05f;
                    foreach (Transform part in fishParts)
                    {
                        if(part.GetComponent<CharacterJoint>())
                            part.GetComponent<CharacterJoint>().breakForce = 0.1f;
                        part.GetComponent<Rigidbody>().drag = 0.05f;
                        part.GetComponent<Rigidbody>().angularDrag = 1;
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
	    if(currHealth <= 0)
        {
            if (deathCountdown <= 0)
            {   //die
                Destroy(gameObject);
            }
            else
            {
                deathCountdown = deathCountdown - 1;

                //disolve game objects
                float intensity = 1f - (deathCountdown * (1f / 100f));
                //Debug.Log(deathCountdown + " " + intensity);
                
                transform.FindChild("mesh").GetComponent<Renderer>().material.SetFloat("_DissolveIntensity", intensity);
                foreach (Transform part in fishParts)
                {
                    part.FindChild("mesh").GetComponent<Renderer>().material.SetFloat("_DissolveIntensity", intensity - Random.Range(.001f,.05f));
                }
            }
        }
	}
}
