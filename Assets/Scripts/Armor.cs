using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    // Damage structure
    public int[] defPoint = { 1, 2, 3, 4, 5, 6, 7, 10 };
    //public GameObject armorGo;
    // Upgrade
    public int armorLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpgradeArmor()
    {
        armorLevel++;
        spriteRenderer.sprite = GameManager.instance.armorSprites[armorLevel];

        // change stats
    }

    public void SetArmorLevel(int level)
    {
        armorLevel = level;
        spriteRenderer.sprite = GameManager.instance.armorSprites[armorLevel];
    }
}
