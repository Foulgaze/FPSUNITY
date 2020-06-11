
using System;
using TMPro;
using UnityEngine;


public class Gun : MonoBehaviour  
{
    public float damage = 5f;
    public float range = 100f;
    public GameObject impactEffect;
    //public GameObject shootingParticle;
    GameObject HatController;
    GunDamage dmg;

    public ParticleSystem muzzleFlash;
    public float fireRate = 7f;

    private float nextTimeToFire = 0f;
    public Camera fpsCam;

    public int bulletMax = 10;
    public int bulletCount;

    public Canvas ammoVisual;
    public float reloadTime = 2.5f;
    public Boolean reloading;
    public float timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        //assignAuthorityObj.GetComponent<NetworkIdentity>().AssignClientAuthority(this.transform.parent.parent.GetComponent<NetworkIdentity>().connectionToClient);
        bulletCount = bulletMax;
        reloading = false;
        HatController = transform.parent.Find("HatController").gameObject;
    }

    private void Awake()
    {
        bulletCount = bulletMax;
        dmg = transform.parent.parent.GetComponent<GunDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {

            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        if (reloading)
        {
            timeLeft -= Time.deltaTime;
        }
        if (timeLeft < 0 && reloading)
        {
            reloading = false;
            bulletCount = bulletMax;
            setAmmo();

        }
        if (!reloading && Input.GetKeyDown("r") && bulletCount != bulletMax)
        {
            startReloading(2.5f);
        }
        if (!reloading)
        {
            transform.parent.Find("Shotgun").GetComponent<AudioSource>().Stop();
        }
    }
    void startReloading(float time)
    {
        transform.GetComponents<AudioSource>()[1].Play();
        timeLeft = time;
        reloading = true;

    }

    void Shoot()
    {
        if(bulletCount > 0 && !reloading)
        {
            bulletCount -= 1;
            setAmmo();

            muzzleFlash.Play();
            AudioSource.PlayClipAtPoint(transform.GetComponents<AudioSource>()[0].clip, transform.position, 0.2f);

            RaycastHit hit;
            //GameObject shootEffect = Instantiate(shootingParticle, shootingLocation.transform.position, shootingLocation.transform.rotation);
            //Destroy(shootEffect, 1f);
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {

                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    if(target.health - (int) Math.Round(damage) <= 0)
                    {
                        GameObject hatcont = hit.transform.GetChild(2).Find("HatController").gameObject;
                        if(hatcont != null)
                        {
                            
                            if(hatcont.transform.childCount > 0)
                            {
                                Debug.Log("TRUEE2E");
                                GameObject tempHat = hatcont.transform.GetChild(hatcont.transform.childCount - 1).gameObject;
                                

                                Vector3 hatPos;
                                hatPos = HatController.transform.position;
                                if (HatController.transform.childCount > 0)
                                {
                                    hatPos = HatController.transform.GetChild(HatController.transform.childCount - 1).GetChild(1).transform.position;
                                }
                             
                                tempHat.transform.rotation = HatController.transform.rotation;
                                tempHat.transform.position = hatPos;
                                tempHat.transform.parent = transform.parent.GetChild(4).transform;
                                Debug.Log(HatController);
                            }
                        }
                    }
                    Debug.Log("SHOOTING: " + hit.transform.name);
                    dmg.lowerGun(hit.transform.name, (int) damage);
                                        
                    //target.TakeDamage((int)Math.Round(damage));
                } else if (hit.transform.tag.Equals("Grenade"))
                {
                    hit.transform.parent.GetComponent<Grenade>().Explode();
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
        else if (!reloading)
        {
            startReloading(2.5f);
        }


    }

    void setAmmo()
    {
        ammoVisual.GetComponent<TextMeshProUGUI>().SetText(bulletCount + "/" + bulletMax);
    }

    

    
}
