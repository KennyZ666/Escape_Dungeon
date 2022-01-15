using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitpoints = 10;
    public int maxHitpoint = 10;
    public int ragepoints = 0;
    public int maxRagepoint = 10;
    public int armor;
    public int movementSpeed = 100;
    public float pushRecoverySpeed = 0.2f;
    public GameObject bloodEffect;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    // Push
    protected Vector3 pushDirection;

    //protected virtual void PlayerArmor(Armor playerArmor)
    //{
        
    //}

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            int receivedDmg = dmg.damageAmount - armor;
            if (receivedDmg < 0)
            {
                receivedDmg = 0;
            }
            hitpoints -= receivedDmg;
            //GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
            // When normalized, a vector keeps the same direction but its length is 1.0.
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText(receivedDmg.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);
            if (receivedDmg > 0 && ragepoints < maxRagepoint)
            {
                //GameManager.instance.ShowText(" + " + 1 + " RAGE POINTS", 25, new Color(150f / 255f, 215f / 255f, 215f / 255f), transform.position, Vector3.up * 50, 1.0f);
                ragepoints++;
            }
            if (hitpoints <= 0)
            {
                hitpoints = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}
