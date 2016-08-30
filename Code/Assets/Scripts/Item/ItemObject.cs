using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemObject : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    new private Text name;
    [SerializeField]
    private Text info;
    [SerializeField]
    private Text amount;

    [SerializeField]
    private int index;

    public void SetInfo(Item item, int _index)
    {
        icon.sprite = item.icon;
        name.text = item.name;
        info.text = item.info;
        amount.text = "x" + item.amount;
        index = _index;
    }

    public void ItemTransfer()
    {
        HUDInterface.instance.chest.RemoveItem(index);
        HUDInterface.instance.ResetLoot();
    }
}
