using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Mover
{
    //public Armor playerArmor;
    //public int armorLevel;
    //private int SelfArmor = 1;
    //private Animator anim;

    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;
    public float lastRageGain;
    public float rageGainCD = 1.0f;
    //public int RagePoints;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager.instance.OnHitpointChange();
        GameManager.instance.OnRagepointChange();
        //movementSpeed = GameManager.instance.moveSpeed;
        //anim = GetComponent<Animator>();

        //DontDestroyOnLoad(gameObject);
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
        {
            return;
        }
        base.ReceiveDamage(dmg);
        if (Time.time - lastRageGain > rageGainCD)
        {
            lastRageGain = Time.time;
            RagePointUpdate(1);
            GameManager.instance.ShowText(" + " + 1 + " RAGE POINTS", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
        }

        GameManager.instance.OnRagepointChange();
        GameManager.instance.OnHitpointChange();
    }

    private void FixedUpdate()
    {
        //assign
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (isAlive)
        {
            UpdateMotor(new Vector3(x, y, 0));
        }

        armor = GameManager.instance.armorAmounts[GameManager.instance.armorLevel];
    }

    public void SwapSprite(int skinID)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }

    //public void SwapAnim(int skinID)
    //{
    //    anim.runtimeAnimatorController = GameManager.instance.animatorControllers[skinID];
    //}

    public void OnLevelUp()
    {
        maxHitpoint++;
        maxRagepoint++;
        hitpoints = maxHitpoint;
        movementSpeed += 5;

        //SelfArmor++;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level - 1; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(int healingAmount)
    {
        if (hitpoints == maxHitpoint)
        {
            return;
        }
        hitpoints += healingAmount;
        if (hitpoints > maxHitpoint)
        {
            hitpoints = maxHitpoint;
        }
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();
    }

    public void RagePointUpdate(int SpentRagePoint)
    {
        ragepoints += SpentRagePoint;
        if (ragepoints > maxRagepoint)
        {
            ragepoints = maxRagepoint;
        }
        if (ragepoints < 0)
        {
            ragepoints = 0;
        }
        GameManager.instance.OnRagepointChange();
    }

    protected override void Death()
    {
        ragepoints = 0;
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
    }

    public void Respawn()
    {
        Heal(maxHitpoint);
        isAlive = true;
        lastImmune = Time.time;
        lastRageGain = Time.time;
        pushDirection = Vector3.zero;
    }

    public void Restart()
    {
        maxHitpoint = 10;
        maxRagepoint = 10;
        hitpoints = 10;
        ragepoints = 0;
        movementSpeed = 200;
        isAlive = true;
        lastImmune = Time.time;
        lastRageGain = Time.time;
        pushDirection = Vector3.zero;
    }
}
