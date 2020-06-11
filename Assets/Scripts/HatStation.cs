using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HatStation : NetworkBehaviour
{
    public bool hasHat = true;
    float timerCount;
    float rehatTime = 1f;
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

        if (timerCount > rehatTime && hasHat == false)
        {
            hasHat = true;
            CmdupdateHat();
            timerCount = 0;
        }
    }

    [Command]
    public void CmdupdateHat()
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
