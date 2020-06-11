﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pellet : MonoBehaviour
{
    float nextTimeToFire = .1f;
    float curTime = 0;
    bool destroyable = false;
    public GunDamage gun;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(curTime > nextTimeToFire)
        {
            destroyable = true;
        }
        else
        {
            curTime += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<CharacterController>() != null || collision.transform.GetComponent<Target>() != null)
        {   
            Debug.Log("We're In");
            gun.lowerGun(collision.transform.name, 6);
            Debug.Log("Pellet Hit: " + collision.transform.name);
            //collision.transform.GetComponent<Target>().TakeDamage((int) 3);
            Destroy(gameObject);
        }
        //if(collision.transform.parent.GetComponent<Grenade>() != null)
        //{
        //    collision.transform.parent.GetComponent<Grenade>().Explode();
        //}
        if (destroyable)
        {
            Destroy(gameObject);
        }

        
        
    }
}
