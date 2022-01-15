using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int pesosAmount = 5;
    public int gemAmount = 0;
    public int diamondAmount = 0;
    public int keysAmount = 0;

    protected override void Oncollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.pesos += pesosAmount;
            GameManager.instance.gems += gemAmount;
            GameManager.instance.diamonds += diamondAmount;
            GameManager.instance.keys += keysAmount;
            if (pesosAmount > 0)
            {
                GameManager.instance.ShowText("+" + pesosAmount + " pesos!", 25, new Color(255,215,0,255), transform.position, Vector3.up * 50, 1.5f);
            }
            if (gemAmount > 0)
            {
                GameManager.instance.ShowText("+" + gemAmount + " Gems!", 25, new Color(24f/255f, 150f/255f, 25f/255f), transform.position, Vector3.up * 50, 1.5f);
            }
            if (diamondAmount > 0)
            {
                GameManager.instance.ShowText("+" + diamondAmount + " Diamonds!", 25, new Color(150f/255f, 215f/255f, 215f/255f), transform.position, Vector3.up * 50, 1.5f);
            }
            if (keysAmount > 0)
            {
                GameManager.instance.ShowText("+" + keysAmount + " Keys!", 25, Color.red, transform.position, Vector3.up * 50, 1.5f);

            }
        }
    }

}
