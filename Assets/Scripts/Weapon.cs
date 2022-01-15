using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage structure
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 99999};
    public float[] pushForce = { 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 2.9f, 3.0f};
    //public float cooldown = 0.5f;

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    private Animator anim;
    //basic att Swing
    public float cooldown = 0.5f;
    private float lastSwing;
    //private bool IsSwing;
    //skill Swordplay
    public float SwordPlayCD = 2.0f;
    private float lastSwordPlay;
    public int SwordPlayCost = 5;
    //skill Stab
    public float StabCD = 1.5f;
    private float lastStab;
    public int StabCost = 3;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //lastSwing = Time.time;
        //lastStab = Time.time;
        //lastSwordPlay = Time.time;
    }

    protected override void Start()
    {
        base.Start();
        
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        int RP = GameManager.instance.player.ragepoints;
        //IsSwing = false;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastSwing > cooldown)
        {
            lastSwing = Time.time;
            //IsSwing = true;
            Swing();

        }
        else if (Input.GetKeyDown(KeyCode.C) && Time.time - lastSwordPlay > SwordPlayCD)
        {
            if (RP < SwordPlayCost)
            {
                GameManager.instance.ShowText("NOT ENOUGH RAGE", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
                return;
            }
            lastSwordPlay = Time.time;
            SwordPlay();
            GameManager.instance.player.RagePointUpdate(-SwordPlayCost);
        }
        else if (Input.GetKeyDown(KeyCode.V) && Time.time - lastStab > StabCD)
        {
            if (RP < StabCost)
            {
                GameManager.instance.ShowText("NOT ENOUGH RAGE", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
                return;
            }
            lastStab = Time.time;
            Stab();
            GameManager.instance.player.RagePointUpdate(-StabCost);
        }
    }

    protected override void Oncollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
            {
                return;
            }

            if (Time.time - GameManager.instance.player.lastRageGain > GameManager.instance.player.rageGainCD)
            {
                GameManager.instance.player.lastRageGain = Time.time;
                GameManager.instance.ShowText(" + " + 1 + " RAGE POINTS", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
                GameManager.instance.player.RagePointUpdate(1);
            }
            
            //if (IsSwing)
            //{
            //    GameManager.instance.ShowText(" + " + 1 + " RAGE POINTS", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
            //    GameManager.instance.player.RagePointUpdate(1);
            //}
            // create a new damage object, then we'll send this object to the enemy
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);

            //Debug.Log(coll.name);
        }
    }

    private void Swing()
    {
        anim.SetTrigger("Swing");
        //anim.SetTrigger("Att_Animation");
    }

    private void SwordPlay()
    {
        anim.SetTrigger("SwordPlay");
    }

    private void Stab()
    {
        anim.SetTrigger("Stab");
    }

    

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];


        // change stats
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
