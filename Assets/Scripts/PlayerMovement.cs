using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterController controller;
    public Camera cam;
    

    public float speed = 12f;

    public float gravity = -25f;

    public float jumpHeight = 3f;

    public Boolean hasJumped = false;

    Vector3 hatPos;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    float timeUntilBonuce;

    GameObject hatSpot;

    Vector3 velocity;
    bool isGrounded;
    void Start()
    {
        hatSpot = gameObject.transform.GetChild(4).gameObject;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            HatStation hatStation = hit.gameObject.GetComponent<HatStation>();
            if (hatStation.hasHat == true)
            {
                hatStation.hasHat = false;
                hatStation.updateHat();
                hatPos = hatSpot.transform.position;
                GameObject tempHat = Instantiate(hatStation.getHat(), hatSpot.transform);
                tempHat.transform.SetParent(hatSpot.transform);
                tempHat.transform.rotation = hatSpot.transform.rotation;
                tempHat.transform.position = hatPos;
                
               //tempHat.transform.parent = hatSpot.transform;
            }
        }
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
