using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private int maxHealth;

    [SyncVar]
    private bool _isDead = false;

    
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;


    private bool[] wasEnabled;

    [SyncVar]
    private int currentHealth;

    public Target target;

    PlayerMovement playerMove;

    public int hatCount = 0;

    TextMeshProUGUI textMesh;

  
    

    public void Setup()
    {

        wasEnabled = new bool[disableOnDeath.Length];
        for(int i =0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        
        
        SetDefaults();
        
    }
     void Start()
    {
        playerMove = transform.GetComponent<PlayerMovement>();
        maxHealth = target.health;
        textMesh = transform.GetChild(0).GetChild(4).transform.GetComponent<TextMeshProUGUI>();
        textMesh.enabled = false;
      
        
    }

    public int currHealth()
    {
        return currentHealth;
    }

    [ClientRpc]
    public void RpcsetVictory()
    {
        Dictionary<string, PlayerManager> dict = GameManager.getDict();
        foreach(string playerID in dict.Keys)
        {
            dict[playerID].textMesh.enabled = true;
            dict[playerID].playerMove.cam.transform.GetComponent<MouseLook>().enabled = false;
            dict[playerID].playerMove.enabled = false;
            if (playerID.Equals(transform.name))
            {
                textMesh.text = "You won the Hattle Royale!";
            }
            else
            {
                dict[playerID].textMesh.text = "YOU LOST. " + transform.name + " Won!";
            }
        }
        
    }

    //public void weaponSwitch(GameObject currWeapon, GameObject switWeapon)
    //{
    //    GameManager.CmdWeaponSwitch(currWeapon, switWeapon, transform.name);
    //}

    public void SetDefaults()
    {
        isDead = false;
        target.health = maxHealth;
        currentHealth = maxHealth;
        target.healthBar.SetHealth(currentHealth);
        for(int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if(_col != null)
        {
            _col.enabled = true;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        SetDefaults();
        Transform _startPoints = NetworkManager.singleton.GetStartPosition();
        transform.position = _startPoints.position;
        transform.rotation = _startPoints.rotation;
    }

    [ClientRpc]
    public void RpcTakeDamager(int damage)
    {
        if (isDead)
        {
            return;
        }
         
        target.TakeDamage(damage);
        currentHealth = target.health;
        if(currentHealth <= 0)
        {

            Die();
        }
    }

    private void Die()
    {

        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        StartCoroutine(Respawn());
        Debug.Log(transform.name + " is dead");


    }
    [Command]
    public void CmdDestroyHat()
    {
        if(playerMove.hatSpot.transform.GetChildCount() > 0)
        {
            GameObject.Destroy(playerMove.hatSpot.transform.GetChild(playerMove.hatSpot.transform.GetChildCount() - 1).gameObject);
            hatCount -= 1;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
