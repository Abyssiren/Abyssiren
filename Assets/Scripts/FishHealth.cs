using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FishHealth : MonoBehaviour {
	public bool isTitleFish = false;
    private ColorEnum _color;
    public ColorEnum healthColor
    {
        get { return _color; }
        set
        {
            _color = value;
        }
    }
    public float health = 100;
    private float currHealth;
    private int deathCountdown = 200;
    public Transform controller;
    //death dissolve 
    private Material mat;
    public AudioClip punchedSound;
    public AudioClip deathSound;


    [SerializeField]
    private Transform[] fishParts;

    public float stun;
    public float currStun;
	// Use this for initialization
	void Start () {
        currHealth = health;
        //mat = gameObject.GetComponent<Renderer>().material;
        healthColor = ColorUtil.getRandomAvailableColor();
        foreach (Transform part in fishParts)
        {
            part.FindChild("mesh").GetComponent<Renderer>().material.color = ColorUtil.getUIImageColorClassFromColorEnum(healthColor);
        }

    }
    //this is now pointless
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
                Broken();
            }
        }
    }
    public void Broken()
    {
		if (isTitleFish) {
			SceneManager.LoadScene (1);
		}
        //you dead. If this is the first time this is called, when the countdown hasn't started, do it.
        if (deathCountdown == 200)
        {
            AudioSource audio = this.gameObject.GetComponent<AudioSource>();
            if (audio == null)
            {
                audio = this.gameObject.AddComponent<AudioSource>();
            }
            audio.Stop();
            audio.PlayOneShot(this.deathSound);

            currHealth = 0;
            //disable AI
                controller.GetComponent<FishEnemyController>().enabled = false;
                foreach (Transform part in fishParts)
                {
                    if (part.GetComponent<CharacterJoint>())
                        part.GetComponent<CharacterJoint>().breakForce = 0.1f;
                    part.GetComponent<Rigidbody>().drag = 0.05f;
                    part.GetComponent<Rigidbody>().angularDrag = 1;
                }
        }
    }
    //children call this to have the fish take damage
    public void TakeDamage(Collision collision)
    {
        GameObject bullet = collision.gameObject;
        ColorEnum bulletColor = bullet.GetComponent<Bullet>().color;
        if (bulletColor == healthColor)
        {
            currHealth -= bullet.GetComponent<Bullet>().power * 3;
        }
        //then, tell the bullet to die
        currHealth -= bullet.GetComponent<Bullet>().power;
        currStun += stun;
        controller.GetComponent<FishEnemyController>().currStun = currStun;


        if (currHealth <= 0)
        {
            //disable AI, die
            Broken();
        }
    }

    public void TakeSlap(Collision collision)
    {
        AudioSource audio = this.gameObject.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = this.gameObject.AddComponent<AudioSource>();
        }
        audio.Stop();
        audio.PlayOneShot(this.punchedSound);

        currStun += stun;
        FishEnemyController cont = controller.GetComponent<FishEnemyController>();
        cont.currStun = currStun;

        //slapped so hard it wants to go home
        cont.countdown = cont.attackTimer;
        currHealth -= 25;


        if (currHealth <= 0)
        {
            //disable AI, die
            Broken();
        }

        float hitSpeed = collision.relativeVelocity.magnitude;

        Debug.Log("stonecoldstunner " + hitSpeed);


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
                float intensity = 1f - (deathCountdown * (1f / 200f));
                
                foreach (Transform part in fishParts)
                {
                    part.FindChild("mesh").GetComponent<Renderer>().material.SetFloat("_DissolveIntensity", intensity - Random.Range(.001f,.05f));
                }
            }
        }
	}
}
