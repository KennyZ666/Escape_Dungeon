using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, attpointText, DefpointText, MovementSpeedText, pesosText, DiamondsText, GemText, KeysText, Weapon_upgradeCostText, Armor_upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public Image armorSprite;
    public RectTransform xpBar;

    private void Start()
    {
        currentCharacterSelection = GameManager.instance.currSkinID;
        //Debug.Log(currentCharacterSelection);
        //Debug.Log(GameManager.instance.currSkinID);
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
    }
    // Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            // if we went too far away
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            // if we went too far away
            if (currentCharacterSelection < 0)
            {
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }

            OnSelectionChanged();
        }
        GameManager.instance.currSkinID = currentCharacterSelection;
        //Debug.Log(GameManager.instance.currSkinID);
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        //GameManager.instance.player.SwapSprite(currentCharacterSelection);
        //GameManager.instance.player.SwapAnim(currentCharacterSelection);
    }

    // Weapon upgrade

    public void OnUpgradeClick_Weapon()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    public void OnUpgradeClick_Armor()
    {
        if (GameManager.instance.TryUpgradeArmor())
        {
            GameManager.instance.armorLevel++;
            UpdateMenu();
        }
    }

    // Update the character Info

    public void UpdateMenu()
    {
        // Weapon & armor
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        armorSprite.sprite = GameManager.instance.armorSprites[GameManager.instance.armor.armorLevel];
        

        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
        {
            Weapon_upgradeCostText.text = "MAX";
        }
        else
        {
            Weapon_upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }
        if (GameManager.instance.armorLevel == GameManager.instance.armorPrices.Count)
        {
            Armor_upgradeCostText.text = "MAX";
        }
        else
        {
            Armor_upgradeCostText.text = GameManager.instance.armorPrices[GameManager.instance.armorLevel].ToString();
        }
        

        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoints.ToString();
        attpointText.text = GameManager.instance.weapon.damagePoint[GameManager.instance.weapon.weaponLevel].ToString();
        DefpointText.text = GameManager.instance.armorAmounts[GameManager.instance.armorLevel].ToString();
        MovementSpeedText.text = (GameManager.instance.player.movementSpeed).ToString();

        pesosText.text = GameManager.instance.pesos.ToString();
        DiamondsText.text = GameManager.instance.diamonds.ToString();
        GemText.text = GameManager.instance.gems.ToString();
        KeysText.text = GameManager.instance.keys.ToString();

        // xp Bar
        int curLevel = GameManager.instance.GetCurrentLevel();
        if (curLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points"; //Display total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpLevel(curLevel - 1);
            int currLevelXp = GameManager.instance.GetXpLevel(curLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff.ToString();
        }
    }
}
