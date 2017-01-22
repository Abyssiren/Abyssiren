using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    //countdown is between waves
    public GameObject player;
    public int countdown;
    public int waveCap;
    [SerializeField]
    public GameObject[] weakEnemies;

    [SerializeField]
    public GameObject[] strongEnemies;
    
    float currCount;
    int waveNum = 1;
    private System.Collections.Generic.List<GameObject> CurrEnemies = new System.Collections.Generic.List<GameObject>();

    // Use this for initialization
    void Start () {
        currCount = countdown;

        //centerPos = new Vector3((Screen.width / 2), (Screen.height / 2), distance);

    }

    // Update is called once per frame
    void FixedUpdate () {
        if(CurrEnemies.Count <= 0)
        {
            if (currCount <= 0)
            {
                //do spawning things
                for (int i = 1; i <= waveNum; i++)
                {

                    float randW = (Random.value - 1 / 2) * 200;
                    float randH = (Random.value - 1 / 2) * 200;
                    float distance = Random.Range(10, 40);

                    Vector3 spawn = Random.onUnitSphere * Random.Range(30, 60) + player.transform.position;
                    GameObject instantiatedEnemy;
                    int rand = Random.Range(0, weakEnemies.Length);
                    instantiatedEnemy = Instantiate(weakEnemies[rand],
                                                                spawn,
                                                                Random.rotation)
                    as GameObject;
                    if (instantiatedEnemy.transform.FindChild("fishhead").GetComponent<FishEnemyController>())
                    {
                        Debug.Log("yeah found the child");
                        FishEnemyController cont = instantiatedEnemy.transform.FindChild("fishhead").GetComponent<FishEnemyController>();
                        cont.distance = distance;
                        cont.attackTimer = Random.value * 400 + 200;
                        cont.entryDelay = Random.Range(0, 8);
                        cont.random = true;
                    }
                    CurrEnemies.Add(instantiatedEnemy);
                }

                waveNum = waveNum + 1;
                //wave limit
                if (waveNum > waveCap)
                {
                    waveNum = waveCap;
                }
                currCount = countdown;
            }
            else
                currCount = currCount - Time.deltaTime;
        }
        else
        {
            int i = CurrEnemies.Count - 1;
            while (i >= 0)
            {
                if (CurrEnemies[i] == null || CurrEnemies[i].Equals(null))
                    CurrEnemies.RemoveAt(i);
                i--;
            }
        }
    }
}
