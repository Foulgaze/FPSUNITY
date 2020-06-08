﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatStation : MonoBehaviour
{
    public bool hasHat = true;
    float timerCount;
    float rehatTime = 30f;
    public GameObject currentHat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHat)
        {
            timerCount += Time.deltaTime;
        }
        
        if(timerCount > rehatTime && hasHat == false)
        {
            hasHat = true;
            updateHat();
            timerCount = 0;
        }
    }

    public void updateHat()
    {
        if (hasHat)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public GameObject getHat()
    {
        return currentHat;
    }
}