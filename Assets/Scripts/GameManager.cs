using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //asign the instance to ourself
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        instance = this;
        //SceneManager.sceneLoaded += SaveState;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

        //DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(hitpointBar.gameObject);
    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<Sprite> armorSprites;
    public List<int> weaponPrices;
    public List<int> armorPrices;
    public List<int> armorAmounts;
    public List<int> xpTable;
    //public List<AnimatorController> animatorControllers;

    //References
    public Player player;

    public Weapon weapon;
    public Armor armor;
    public int currSkinID;
    public int armorLevel;
    //public int moveSpeed;
    public FloatingTextManager floatingTextManager;
    public GameObject menu;
    public GameObject hud;
    public RectTransform hitpointBar;
    public RectTransform ragepointBar;
    public Text curr_HP_text, curr_RP_text;
    public Animator deathMenuAnim;
    public Animator lvlupAnim;

    //Logic
    public int pesos;
    public int gems;
    public int diamonds;
    public int keys;
    public int experience;


    //floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    public bool TryUpgradeArmor()
    {
        // is the Armor max level?
        if (armorPrices.Count <= armor.armorLevel)
        {
            return false;
        }

        if (pesos >= armorPrices[armor.armorLevel])
        {
            pesos -= armorPrices[armor.armorLevel];
            armor.UpgradeArmor();
            return true;
        }
        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoints / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(ratio, 1, 1);
        curr_HP_text.text = GameManager.instance.player.hitpoints.ToString() + " / " + GameManager.instance.player.maxHitpoint.ToString();
    }

    public void OnRagepointChange()
    {
        float ratio = (float)player.ragepoints / (float)player.maxRagepoint;
        ragepointBar.localScale = new Vector3(ratio, 1, 1);
        curr_RP_text.text = GameManager.instance.player.ragepoints.ToString() + " / " + GameManager.instance.player.maxRagepoint.ToString();
    }

    // experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) //max level
            {
                return r;
            }
        }

        return r;
    }
    public int GetXpLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        Debug.Log("Level Up");
        player.OnLevelUp();
        OnHitpointChange();
        lvlupAnim.SetTrigger("lvlup");
    }

    // On scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death Menu and Respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    // Save state
    /*
     * INT preferedSkin
     * INT pesos
     * INT GEMS
     * INT DIAMONDS
     * INT KEYS
     * INT experience
     * INT weaponLevel
     * INT armorLevel
    */
    public void SaveState()
    {
        string s = "";

        s += currSkinID.ToString() + "|";
        s += pesos.ToString() + "|";
        s += gems.ToString() + "|";
        s += diamonds.ToString() + "|";
        s += keys.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString() + "|";
        s += armor.armorLevel.ToString();
        //s += moveSpeed.ToString();

        PlayerPrefs.SetString("SaveState", s);
        GameManager.instance.ShowText("Game Saved", 25, Color.white, GameObject.Find("Player").transform.position, Vector3.up * 30, 1.0f);
        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        //change player skin
        currSkinID = int.Parse(data[0]);
        // money
        pesos = int.Parse(data[1]);
        gems = int.Parse(data[2]);
        diamonds = int.Parse(data[3]);
        keys = int.Parse(data[4]);

        //experience
        experience = int.Parse(data[5]);
        if (GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());
        }
        //change the weapon & armor level

        weapon.SetWeaponLevel(int.Parse(data[6]));
        armor.SetArmorLevel(int.Parse(data[7]));
        //moveSpeed = int.Parse(data[8]);

        //player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        //Debug.Log("LoadState");
    }

    public void RestartState()
    {
        // reset database
        string s = "";

        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0" + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
        //reset stats
        currSkinID = 0;
        pesos = 0;
        gems = 0;
        diamonds = 0;
        keys = 0;
        experience = 0;
        if (GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());
        }
        weapon.SetWeaponLevel(0);
        armor.SetArmorLevel(0);

        deathMenuAnim.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Restart();
    }

}
