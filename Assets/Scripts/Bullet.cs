using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour
{
    public int power;
    public ColorEnum color;
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
        this.transform.localScale = originalScale * timeElapsed * 1000.0f;

        if (timeElapsed > 0.5f)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        timeElapsed = 0.0f;
        this.gameObject.SetActive(true);
        this.transform.localScale = originalScale;
    }
}