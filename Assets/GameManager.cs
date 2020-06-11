using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public MatchSettings matchSettings;
    public NetworkManager n;
    GameObject hatStand;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one game manager in scene");
        }
        else
        {
            instance = this;
        }
        
        //NetworkServer.Spawn(hatStand);
        //Debug.Log(NetworkServer.active); 
        //CmdspawnHatMachine(test);

        
    }

    [ClientRpc]
    public void RpcspawnHatMachine()
    {
        GameObject hatSpawn = Instantiate(n.spawnPrefabs[0], new Vector3(5, 2, 5), Quaternion.identity);
        NetworkServer.Spawn(hatSpawn);
    }


    //[Command]
    //public void RpcHatMaker(GameObject hat, Transform trans, Vector3 pos)
    //{

    //    GameObject tempHat = Instantiate(hat, trans);
    //    tempHat.transform.SetParent(trans);
    //    tempHat.transform.rotation = trans.rotation;
    //    tempHat.transform.position = pos;

    //}

       


    #region Player tracking
    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    // Start is called before the first frame update
    
    public static void RegisterPlayer(string netID, PlayerManager player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
        
        //RpcspawnHatMachine(HatStation);
    }

    public static void DeRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Dictionary<string, PlayerManager> getDict()
    {
        return players;
    }

    public static PlayerManager GetPlayer (string playerID)
    {
        return players[playerID];
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200,200,200,500));
        GUILayout.BeginVertical();
        foreach(string playerID in players.Keys)
        {
            GUILayout.Label(playerID + " - " + players[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }



    //[ClientRpc]
    //public static void RpcweaponSwitch(GameObject startingWeapon, GameObject endingWeapon, string playerID)
    //{
    //    GetPlayer(playerID).transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
    //    GetPlayer(playerID).transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
    //}

    
    //void Update()
    //{
    //    foreach (string playerID in players.Keys)
    //    {
    //        if (players[playerID].hatCount >= 3)
    //        {
    //           instance.RpcHatWin(playerID);
    //        }
    //    }
    //}

    //[ClientRpc]
    //void RpcHatWin(string playerID)
    //{

    //    foreach (string playerLost in players.Keys)
    //    {
    //        if (playerID == playerLost)
    //        {
    //            players[playerID].CmdsetVictory("You Won the HattleRoyale!");
    //        }
    //        else
    //        {
    //            players[playerLost].CmdsetVictory(playerID + " has Won!");
    //        }
    //    }
    //    Debug.Log(playerID + "won");
    //}

    public static int dictSize()
    {
        return players.Count;
    }

    //public static void takeDamage(string playerID, int damage)
    //{
    //    GameManager.GetPlayer(playerID).takeDamage(damage)
    //}
    #endregion
}
