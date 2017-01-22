using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Projectile : MonoBehaviour {
	public Rigidbody projectile;
	private Queue<Rigidbody> bullets;
	private int pooledAmount = 10;
	public int fireRate = 1;
	public int power = 25;
	private int currFire;
	public float bulletSpeed = 100;

	public static float shootVolumeTreshold = 0.2f;
	//buffer so you can buffer shots
	private int bufferFrames = 1;
	private int currBuff;

	private bool firing = false;
    public float originy = 0.3f;
	// Use this for initialization
	void Start () {
		currFire = 0;
		currBuff = 0;
		bullets = new Queue<Rigidbody>();
		for (int i = 0; i < pooledAmount; i++)
		{
			Rigidbody instantiatedProjectile = Instantiate(projectile, new Vector3(999,999,9999), new Quaternion(0,0,0,0)) as Rigidbody;
			bullets.Enqueue(instantiatedProjectile);
		}
	}

	// Update is called once per frame
	void Update () {
		//fire, or buffered fireing
		if (Input.GetButtonDown("Fire1") || currBuff > 0 || MicTestFinishedBuild_Input.volumeVal > shootVolumeTreshold)
		{
			firing = true;
			currBuff = currBuff - 1;

			//if firing is at 0, shoot.
			if (currFire <= 0)
			{
				//set the fire rate back up
				currFire = fireRate;

				//generate angle of shooting
				Vector3 origin = transform.position + transform.forward * 2;
				Quaternion rot = transform.rotation;
				Vector3 rotator = rot.eulerAngles;
                origin.y -= originy; // Start projectile slightly lower
				//rotator.z += 90;
				//rot = Quaternion.Euler(rotator);
				//debug.text = rot.eulerAngles.ToString() + "\n " + Camera.main.transform.rotation.eulerAngles.ToString();

				Rigidbody instantiatedProjectile = bullets.Dequeue();

				/*
                Rigidbody instantiatedProjectile = Instantiate(projectile,
                                                               origin,
                                                               rot)
                    as Rigidbody;
                    */
				//reset shit
				instantiatedProjectile.velocity = Vector3.zero;
				instantiatedProjectile.angularVelocity = Vector3.zero;

                Bullet mybullet = instantiatedProjectile.GetComponent<Bullet>();
                mybullet.Reset();
                mybullet.color = ColorUtil.getColorFromVolume(MicTestFinishedBuild_Input.volumeVal);

				//set shit
				instantiatedProjectile.transform.position = origin;
				instantiatedProjectile.transform.rotation = rot;

				instantiatedProjectile.transform.Rotate(90, 0, 0, Space.Self);

				//shooting code
				instantiatedProjectile.AddForce(instantiatedProjectile.transform.forward * bulletSpeed);
				instantiatedProjectile.velocity = transform.forward * bulletSpeed;
				//debug.text += "\n" + instantiatedProjectile.velocity.ToString();
				//debug.text += "\n" + Screen.width.ToString() + " " + Screen.height.ToString();

				bullets.Enqueue(instantiatedProjectile);
			}
			else
			{
				//else, buffer the fire command and continue to count down the firing.
				if (Input.GetButtonDown("Fire1"))
					currBuff = bufferFrames;
				currFire = currFire - 1;
			}
		}
	}
}
