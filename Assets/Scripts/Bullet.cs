using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour
{
    public int power;
    private ColorEnum _color;
    public ColorEnum color
    {
        get { return _color; }
        set
        {
            Renderer rend = GetComponent<Renderer>();
            switch (value)
            {
                case ColorEnum.BLUE:
                    rend.material.color = Color.blue;
                    break;
                case ColorEnum.GREEN:
                    rend.material.color = Color.green;
                    break;
                case ColorEnum.YELLOW:
                    rend.material.color = Color.yellow;
                    break;
                case ColorEnum.RED:
                    rend.material.color = Color.red;
                    break;
                case ColorEnum.ERROR:
                    rend.material.color = Color.black;
                    break;
            }
            _color = value;
        }
    }
    private float maxScale = 100.0f;
    private float scaleAmount = 10.0f;
    private float timeElapsed = 0.0f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = this.transform.localScale;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        this.transform.localScale = originalScale * timeElapsed * 800.0f;

        if (timeElapsed > .5f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void HitboxOff()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    } 

    public void Reset()
    {
        timeElapsed = 0.0f;
        this.gameObject.SetActive(true);
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        this.transform.localScale = originalScale;
    }
}