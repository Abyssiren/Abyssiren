using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FishEnemyController : MonoBehaviour
{
    public AudioClip warningSound;
    private Rigidbody rb;
    public Vector3 speed;
    public float attackTimer;
    //distance before behavior changing
    public Plane threshold;
    public float entryDelay;

    public float countdown;
    public float distance;
    public float initialD;
    public bool random = true;
    public float randW = 0;
    public float randH = 0;
    public float startRotX, startRotY;
    public enum FishType
    {
        Dumb,
        Fast,
        Slow,
        Wait
    }
    public enum FishAI
    {
        Straight,
        Left,
        Right,
        Up,
        Down
    }
    //where the fish going left, right, etc, will turn. So they'll go that far first, then hit a plane at the turning point, and then turn at the player.
    public float turningPoint;
    public float aiDistance;
    public FishAI AI;
    public FishType type;

    public Vector3 startPoint;
    public Vector3 endPoint;
    public float currStun;

    public bool attacked = false;
    private Vector3 playerTrackedPos;
    private Vector3 playerTrackedFor;
    private Vector3 playerTrackedUp;
    private Vector3 playerTrackedRight;
    public Vector3 waitPoint;
    private Vector3 ogScale;
    public float movingSpeed = 8;
    public float attackingSpeed = 8;
    private bool attackSounding = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //give countdown some flavor nvm, don't. strict times are best.
        countdown = attackTimer;
        if (initialD == 0)
            initialD = distance;



        //for fish going to a target.

        //get world point of a different local point, after fixing scale.
        startPoint = (transform.position);
        endPoint = endPoint - transform.localPosition;
        Vector3 scale = transform.localScale;
        scale.Set(1 / scale.x, 1 / scale.z, 1 / scale.z);
        endPoint = Vector3.Scale(endPoint, scale);
        endPoint = transform.TransformPoint(endPoint);

        //might as well give em a little randomness
        randW = Random.value * 360;
        randH = Random.value * 40 - 25;

        


        threshold = new Plane();

        //start this enemy up after the entry delay...
        ogScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        // Show button
        StartCoroutine(ExecuteAfterTime(entryDelay, "begin"));

        //get a position relative to the player in a sphere.
        float trueD;
        if (type == FishType.Wait)
            trueD = initialD;
        else
            trueD = distance;

        GameObject playerObject = GameObject.Find("player");
        if (random)
        {
            //randomly generate
            Quaternion rotation = Quaternion.Euler(randW, randH, 0);
            rotation = transform.rotation * rotation;
            waitPoint = rotation * playerObject.transform.forward * trueD;

        }
        else
        {
            Quaternion rotation = Quaternion.Euler(startRotX, startRotY, 0);

            Debug.Log(gameObject.name + " " + rotation + " " + transform.rotation);
            rotation = transform.rotation * rotation;
            waitPoint = rotation * playerObject.transform.forward * trueD;

            Debug.Log(rotation + " " + playerObject.transform.position + " " + playerObject.transform.forward + " " + waitPoint);
        }

    }
    IEnumerator ExecuteAfterTime(float time, string e)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        switch (e)
        {
            case "begin":
                transform.localScale = ogScale;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    void DrawLine(Vector3 start, Vector3 end, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = new Color(1, 0, 0, .5f);
        lr.endColor = new Color(0.5f, 0.5f, 0.5f, .3f);
        lr.startWidth =.3f;
        lr.endWidth = .03f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }


    void FixedUpdate()
    {
        //check for getting hit.
        
        if (currStun > 0)
        {
            currStun -= 10 * Time.deltaTime;
        }
        else
        {
            float factor;
            Vector3 final;
            GameObject playerObject = GameObject.Find("player");
            switch (type)
            {
                default:
                    // --- waiting, like a dumb ass fish, or a waiting fish.
                    if (countdown > 15 && (type == FishType.Dumb || type == FishType.Wait))
                    {
                        // --- Target position ---
                        

                        
                        Vector3 targetPos = waitPoint;
                        //actual velocity setting
                        final = targetPos - transform.position;

                        //go to it's waiting position. Factor is the speed it goes to the waiting position, I guess.
                        factor = 1;
                        if (final.magnitude < 3)
                            factor = 0.1f;
                        else if (final.magnitude < 10)
                            factor = 0.5f;


                        final = final.normalized * factor;
                        //rb.AddForce(final, ForceMode.VelocityChange);
                        rb.velocity += (((final * movingSpeed) - rb.velocity) * (0.5f));
                        //LOOK AT ME if not going fast!
                        if (rb.velocity.magnitude < 5)
                        {
                            Quaternion q = Quaternion.LookRotation(playerObject.transform.position - transform.position);
                            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, q, 360 * Time.deltaTime));
                        }
                        else
                        {
                            Quaternion q = Quaternion.LookRotation(final);
                            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, q, 360 * Time.deltaTime));
                        }

                        //count down to attack. Don't do if waiting.
                        if (type != FishType.Wait)
                            countdown = countdown - 1;
                    }
                    else
                    {
                        //attack, when countdown is done, or you're not a dumb fish.
                        if(!attackSounding)
                        {
                            AudioSource audio = this.gameObject.AddComponent<AudioSource>();
                            audio.PlayOneShot(this.warningSound);
                            attackSounding = true;
                            DrawLine(transform.position, playerObject.transform.position - playerObject.transform.up + (playerObject.transform.position - transform.position).normalized * 3, .2f);
                        }
                        if(countdown > 0)
                        {
                            //when the countdown is between 15 and 0, the fish is tracking the player's position.
                            playerTrackedPos = playerObject.transform.position;
                            playerTrackedFor = playerObject.transform.forward;
                            playerTrackedUp = playerObject.transform.up;
                            playerTrackedRight = playerObject.transform.right;
                            countdown = countdown - 1;
                            Debug.Log(countdown);
                        }

                        final = (playerTrackedPos - transform.position);

                        //if the fish hits or get close enough to it's target, it's done. Find a position forward that is the distance away, then set your countdown to halfish.
                        if (final.magnitude <= 1)
                        {
                            random = false;
                            waitPoint = transform.forward * distance;
                            countdown = attackTimer;
                            attackSounding = false;
                        }

                        //check if fish has passed the threshold to attack head on, if it has AI.
                        threshold.SetNormalAndPosition(playerObject.transform.forward.normalized, playerObject.transform.position + playerObject.transform.forward.normalized * turningPoint);
                        

                        //if you are any direction, go half way to the player in the direction said. otherwise, go straight.
                        if (final.magnitude < ((distance) * 1 / 2) || AI == FishAI.Straight)
                        {
                            Quaternion q = Quaternion.LookRotation(playerTrackedPos - transform.position);
                            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, q, 100 * Time.deltaTime));
                            Vector3 attackdir = (playerTrackedPos - transform.position).normalized;
                            rb.velocity += (((attackdir * attackingSpeed) - rb.velocity) * (0.5f));
                        }
                        else
                        {
                            Vector3 attackdir = new Vector3();
                            switch (AI)
                            {
                                case FishAI.Up:
                                    attackdir = (playerTrackedPos + (playerTrackedUp * aiDistance) - transform.position).normalized;
                                    rb.velocity += (((attackdir * attackingSpeed) - rb.velocity) * (0.5f));
                                    break;
                                case FishAI.Down:
                                    attackdir = (playerTrackedPos - (playerTrackedUp * aiDistance) - transform.position).normalized;
                                    rb.velocity += (((attackdir * attackingSpeed) - rb.velocity) * (0.5f));
                                    break;
                                case FishAI.Right:
                                    attackdir = (playerTrackedPos + (playerTrackedRight * aiDistance) - transform.position).normalized;
                                    rb.velocity += (((attackdir * attackingSpeed) - rb.velocity) * (0.5f));
                                    break;
                                case FishAI.Left:
                                    attackdir = (playerTrackedPos - (playerTrackedRight * aiDistance) - transform.position).normalized;
                                    rb.velocity += (((attackdir * attackingSpeed) - rb.velocity) * (0.5f));
                                    break;
                            }
                        }
                    }
                    //add drag?
                    rb.velocity *= Mathf.Clamp01(1f - 5 * Time.fixedDeltaTime);
                    break;
            }

           
        }
    }

}
