using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//[ ] Does the item handler have an item variable?
//[ ] Does the item handler have an item type variable?

public class ItemHandler
{
    protected Item item;
    public ItemType itemType;

    void Start()
    {
        item = new Item();
    }

    public void SetItemInformation(Item i){
        if (i == null)
            return;
        item = i.getItem();
        item.copy(i);
    
    }

}

