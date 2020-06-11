using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunDamage : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lowerGun(string playerID, int damage)
    {
        CmdPlayerShot(playerID, damage);
    }


   [Command]
    void CmdPlayerShot(string playerID, int damage)
    {
        PlayerManager player = GameManager.GetPlayer(playerID);
        int tempHealth = player.currHealth();
        player.RpcTakeDamager(damage);
        Debug.Log(player.currHealth());
        Debug.Log(player.isDead);
        if ((tempHealth - damage) <= 0 && !player.isDead && player.hatCount > 0)
        {
            Debug.Log("Adding hat");
            transform.GetComponent<PlayerMovement>().CmdhatAddFromNothing();
            player.CmdDestroyHat();
        }
    }

    //[Command]
    //void CmdPlayerShot(string playerID, int damage)
    //{
    //    GameManager.takeDamage(playerID, damage);
      
    //}
}
