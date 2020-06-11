using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    public Camera cam;
    

    public float speed = 12f;

    public float gravity = -25f;

    public float jumpHeight = 3f;

    public Boolean hasJumped = false;

    Vector3 hatPos;

    public GameObject hatSpawner;
    public GameObject hat;



    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    float timeUntilBonuce;

    public GameObject hatSpot;


    float newHatTimer = 5f;
    float currentHatTimer;
    bool hatTimer = false;

    Vector3 velocity;
    bool isGrounded;
    void Start()
    {
        hatSpot = gameObject.transform.GetChild(2).Find("HatController").gameObject;
        //if (GameManager.dictSize() == 2)
        //{
        //    GameManager.instance.RpcspawnHatMachine();
        //}

    }


    [Command]
    void CmdSpawnHatSpawners()
    {
       // GameObject hatSpawn = Instantiate(hatSpawner, new Vector3(5, 2, 5), Quaternion.identity);
        //NetworkServer.Spawn(hatSpawner);
        RpcSpawnHats();
        
    }

    [ClientRpc]
    void RpcSpawnHats()
    {
        GameObject hatSpawn = Instantiate(hatSpawner, new Vector3(5, 2, 5), Quaternion.identity);
        NetworkServer.Spawn(hatSpawner);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (hatTimer)
        {
            newHatTimer -= Time.deltaTime;
            if(newHatTimer < 0)
            {
                hatTimer = false;
                newHatTimer = 5f;
            }
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
           
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
           
            //Vector3 lemove = new Vector3(cam.transform.rotation.x, 0f, cam.transform.rotation.z);
            controller.Move(cam.transform.forward * 5);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            CmdSpawnHatSpawners();
        }
        

        

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (hasJumped)
        {
            timeUntilBonuce -= Time.deltaTime;
        }
        if(timeUntilBonuce < 0 && hasJumped)
        {
            hasJumped = false;
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag.Equals("GrenadeStation"))
        {
            if(hit.transform.GetComponent<GrenadeStation>().hasGrenade == true)
            {
                transform.GetComponent<GrenadeCount>().increaseGrenade();
                hit.transform.GetComponent<GrenadeStation>().DisableGrenade();
            }
        }
        if (hit.transform.tag.Equals("BouncePad") && !hasJumped)
        {

            bouncePad script = hit.transform.GetComponent<bouncePad>();
            velocity.y = (float) script.jumpForce;
            hasJumped = true;
            timeUntilBonuce = script.waitAfterJump;
        }
        if (hit.transform.tag.Equals("HatStation"))
        {
            //RpchatAdd(hit.transform.gameObject);
            CmdhatAdd(hit.transform.gameObject);
        }
    }

    //[ClientRpc]
    //void RpchatAdd(GameObject hit)
    //{
    //    HatStation hatStation = hit.GetComponent<HatStation>();
    //    if (hatStation.hasHat == true)
    //    {

    //        hatStation.hasHat = false;
    //        hatStation.RpcupdateHat();
    //        hatPos = hatSpot.transform.position;
    //        if (hatSpot.transform.childCount > 0)
    //        {
    //            hatPos = hatSpot.transform.GetChild(hatSpot.transform.childCount - 1).GetChild(1).transform.position;
    //        }
    //        //n.RpcHatMaker(hatStation.getHat(), hatSpot.transform, hatPos);
    //        GameObject tempHat = Instantiate(hatStation.getHat(), hatSpot.transform);
    //        tempHat.transform.SetParent(hatSpot.transform);
    //        tempHat.transform.rotation = hatSpot.transform.rotation;
    //        tempHat.transform.position = hatPos;

    //        //tempHat.transform.parent = hatSpot.transform;
    //    }
    //}

    [Command]
    void CmdhatAdd(GameObject hit)
    {
        //HatStation hatStation = hit.GetComponent<HatStation>();
        //if (hatStation.hasHat == true)
        //{

        //hatStation.hasHat = false;
        //hatStation.CmdupdateHat();
        if (!hatTimer)
        {
            hatTimer = true;
            hatPos = hatSpot.transform.position;
            if (hatSpot.transform.childCount > 0)
            {
                hatPos = hatSpot.transform.GetChild(hatSpot.transform.childCount - 1).GetChild(1).transform.position;
            }
            //n.RpcHatMaker(hatStation.getHat(), hatSpot.transform, hatPos);
            // GameObject tempHat = Instantiate(hatStation.getHat(), hatSpot.transform);
            GameObject tempHat = Instantiate(hat, hatSpot.transform);
            tempHat.transform.SetParent(hatSpot.transform);
            tempHat.transform.rotation = hatSpot.transform.rotation;
            tempHat.transform.position = hatPos;
            Debug.Log(this.gameObject.GetComponent<NetworkIdentity>());
            //NetworkServer.SpawnWithClientAuthority(tempHat, this.gameObject);
            NetworkServer.Spawn(tempHat);
            if ((transform.GetComponent<PlayerManager>().hatCount += 1) > 3)
            {
                transform.GetComponent<PlayerManager>().RpcsetVictory();
            }
            //GameManager.instance.RpchatWin();

            //tempHat.transform.parent = hatSpot.transform;
            // }
        }
    }

    [Command]
    public void CmdhatAddFromNothing()
    {
        hatPos = hatSpot.transform.position;
        if (hatSpot.transform.childCount > 0)
        {
            hatPos = hatSpot.transform.GetChild(hatSpot.transform.childCount - 1).GetChild(1).transform.position;
        }
        //n.RpcHatMaker(hatStation.getHat(), hatSpot.transform, hatPos);
        GameObject tempHat = Instantiate(hat, hatSpot.transform);
        tempHat.transform.SetParent(hatSpot.transform);
        tempHat.transform.rotation = hatSpot.transform.rotation;
        tempHat.transform.position = hatPos;
        Debug.Log(this.gameObject.GetComponent<NetworkIdentity>());
        //NetworkServer.SpawnWithClientAuthority(tempHat, this.gameObject);
        NetworkServer.Spawn(tempHat);
        if((transform.GetComponent<PlayerManager>().hatCount += 1) > 3)
        {
            transform.GetComponent<PlayerManager>().RpcsetVictory();
        }
        
        //GameManager.instance.RpchatWin();


        //tempHat.transform.parent = hatSpot.transform;

    }



    public void removeLine()
    {
        StartCoroutine(lineDelete(gameObject.GetComponent<LineRenderer>()));
    }

    IEnumerator lineDelete(LineRenderer line)
    {

        yield return new WaitForSeconds(0.5f);
        line.SetVertexCount(0);
        //Do Function here...
    }
}
