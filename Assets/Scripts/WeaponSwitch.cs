using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponSwitch : NetworkBehaviour
{
    public Camera cam;
    public Canvas ammoVisual;
    int bulletMax;
    int bulletCount;
    bool[] firstSwitch = new bool[2];


    
    private void Start()
    {
        if (!isLocalPlayer)
            return;
        CmdChangeWeapon(0);
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown("2"))
        {
            CmdChangeWeapon(1);
        }
        if (Input.GetKeyDown("1"))
        {
            CmdChangeWeapon(0);
        }
        if (Input.GetKeyDown("3"))
        {
            CmdChangeWeapon(2);
        }
        if (Input.GetKeyDown("4"))
        {
            CmdChangeWeapon(3);
        }
    }


    [Command]
    public void CmdChangeWeapon(int weaponNO)
    {

        int count = cam.transform.GetChildCount();
        string weaponName;
        for (int i = 0; i < count; i++)
        {
            if (!cam.transform.GetChild(i).gameObject.tag.Equals("HatController")) 
            {
                cam.transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }

            
        switch (weaponNO)
        {
            case 0: weaponName = "Gun"; break;
            case 1: weaponName = "Grenade"; break;
            case 2: weaponName = "Sniper"; break;
            case 3: weaponName = "Shotgun"; break;
            default:weaponName = "Gun";break;

        }
        if(weaponName == "Sniper")
        {
            //This is considered the 2nd weapon not counting grenade
            bulletMax = cam.transform.Find(weaponName).GetComponent<Sniper>().bulletMax;
            bulletCount = cam.transform.Find(weaponName).GetComponent<Sniper>().bulletCount;
            cam.transform.Find(weaponName).GetComponent<Sniper>().reloading = false;
            //transform.GetComponent<PlayerManager>().weaponSwitch(transform.GetChild(2).GetChild(0).gameObject, transform.GetChild(2).GetChild(2).gameObject);




            ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletMax);
        }
        if(weaponName == "Gun")
        {

            bulletMax = cam.transform.Find(weaponName).GetComponent<Gun>().bulletMax;
            bulletCount = cam.transform.Find(weaponName).GetComponent<Gun>().bulletCount;
            cam.transform.Find(weaponName).GetComponent<Gun>().reloading = false;
            ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletMax);
        }
        if (weaponName == "Shotgun")
        {
            bulletMax = cam.transform.Find(weaponName).GetComponent<Shotgun>().bulletMax;
            bulletCount = cam.transform.Find(weaponName).GetComponent<Shotgun>().bulletCount;
            cam.transform.Find(weaponName).GetComponent<Shotgun>().reloading = false;
            ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletMax);
        }
        //ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletCount);
        GameObject weapon;
        try
        {
            weapon = cam.transform.Find(weaponName).gameObject;

        } catch(NullReferenceException e)
        {
            Debug.Log("Not find");
             weapon = cam.transform.Find("Gun").gameObject;
            bulletMax = cam.transform.Find("Gun").GetComponent<Gun>().bulletMax;
            bulletCount = cam.transform.Find("Gun").GetComponent<Gun>().bulletCount;
            cam.transform.Find("Gun").GetComponent<Gun>().reloading = false;
            ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletMax);
        }
        weapon.SetActive(true);
        Debug.Log(weapon);
        RpcWork(weapon.name);
        
    }

    [ClientRpc]
    void RpcWork(String weaponName)
    {
        int count = cam.transform.GetChildCount();
        for (int i = 0; i < count; i++)
        {
            if (!cam.transform.GetChild(i).gameObject.tag.Equals("HatController"))
            {
                cam.transform.GetChild(i).gameObject.SetActive(false);
            }

        }

        cam.transform.Find(weaponName).gameObject.SetActive(true);
    }
}
