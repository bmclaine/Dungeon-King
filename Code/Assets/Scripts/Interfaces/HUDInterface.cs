using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HUDInterface : MonoBehaviour
{
    private static HUDInterface hud;
    public static HUDInterface instance
    {
        get
        {
            if (hud)
                return hud;

            hud = FindObjectOfType<HUDInterface>();

            return hud;
        }
    }

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image manaBar;
    [SerializeField]
    private Image expBar;
    [SerializeField]
    private Image elementDisplay;
    [SerializeField]
    private Image abilityCooldown;
    [SerializeField]
    private Image potionDisplay;
    [SerializeField]
    private Text potionCount;
    [SerializeField]
    private Image weaponDisplay;
    [SerializeField]
    private Image bossHealthBar;
    [SerializeField]
    private GameObject bossHealthFrame;
    [SerializeField]
    private Image messageWindow;
    [SerializeField]
    private Text messageText;

    [SerializeField]
    private Sprite[] elements;
    [SerializeField]
    private Sprite emptyIcon;
    [SerializeField]
    private ItemObject[] itemObj;
    [SerializeField]
    private GameObject lootWindow;

    private float expValue;

    [HideInInspector]
    public Chest chest;


    void Start()
    {
        bossHealthFrame.gameObject.SetActive(false);
    }

    void Update()
    {
        if (expValue > expBar.fillAmount)
        {
            
            expBar.fillAmount = Mathf.MoveTowards(expBar.fillAmount, expValue, Time.deltaTime);

            if (expValue - expBar.fillAmount < 0.01f)
            {
                expBar.fillAmount = expValue;
                if (expValue >= 1.0f)
                {
                    expValue = 0.0f;
                    expBar.fillAmount = 0.0f;
                    EntityManager.instance.player.LevelUp();
                }
            }
        }
    }

    public void SetHealthBar(float value)
    {
        if (healthBar == null) return;

        healthBar.fillAmount = value;
    }

    public void SetManaBar(float value)
    {
        if (manaBar == null) return;

        manaBar.fillAmount = value;
    }

    public void SetExpBar(float value)
    {
        if (expBar == null) return;

        expValue = value;
        if (expValue < expBar.fillAmount)
        {
            expBar.fillAmount = value;
        }
    }

    public void SetElementDisplay(Element element)
    {
        int index = (int)element;

        elementDisplay.sprite = elements[index];
    }

    public void SetPotionQuickslot(Potion potion)
    {
        if (potion == null)
            potionDisplay.sprite = emptyIcon;

        potionDisplay.sprite = potion.icon;

        if (potion != null)
        {
            potionCount.text = "x" + potion.amount.ToString();
        }
    }

    public void ResetPotionQuickslot()
    {
        potionDisplay.sprite = emptyIcon;
        potionCount.text = " ";
    }

    public void SetWeaponQuickslot(Weapon weapon)
    {
        weaponDisplay.sprite = weapon.icon;
    }

    public void SetBossHealthBar(float value)
    {
        if (bossHealthBar == null) return;

        bossHealthFrame.gameObject.SetActive(true);

        if (value <= 0.0f)
            bossHealthFrame.gameObject.SetActive(false);

        bossHealthBar.fillAmount = value;
    }

    public void SetCooldown(float value)
    {
        abilityCooldown.fillAmount = value;
    }

    public void ResetLoot()
    {
        if (!chest || !lootWindow) return;

        ClearLoot();

        for (int i = 0; i < chest.itemList.Count; i++)
        {
            itemObj[i].gameObject.SetActive(true);
            itemObj[i].SetInfo(chest.itemList[i], i);
        }

        lootWindow.SetActive(true);

        if(EventSystem.current)
            itemObj[0].GetComponent<Button>().Select();

        if (chest.itemList.Count <= 0)
        {
            lootWindow.SetActive(false);
        }
    }

    public void ClearLoot()
    {
        if (!lootWindow) return;

        lootWindow.SetActive(false);

        for (int i = 0; i < itemObj.Length; i++)
        {
            itemObj[i].gameObject.SetActive(false);
        }
    }

    public void SetMessageWindow(string str)
    {
        messageText.text = str;

        Color windowCol = messageWindow.color;
        windowCol.a = 1;
        messageWindow.color = windowCol;

        Color textCol = messageText.color;
        textCol.a = 1;
        messageText.color = textCol;

        messageWindow.gameObject.SetActive(true);
    }

    public void ClearMessageWindow()
    {
        messageWindow.gameObject.SetActive(false);
    }

    public float FadeMessage()
    {
        Color windowCol = messageWindow.color;
        windowCol.a -= Time.deltaTime * 0.3f;
        messageWindow.color = windowCol;

        Color textCol = messageText.color;
        textCol.a -= Time.deltaTime * 0.3f;
        messageText.color = textCol;

        return messageWindow.color.a;
    }
}
