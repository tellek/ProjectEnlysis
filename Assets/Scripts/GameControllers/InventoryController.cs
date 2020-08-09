using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    [HideInInspector]
    public List<InventoryItem> Items;

    public void AddItem(InventoryItem item)
    {
        DoTheAdd(item.Name, item.Amount);
    }

    public void AddItem(string name, int amount)
    {
        DoTheAdd(name, amount);
    }

    public void RemoveItem(InventoryItem item)
    {
        DoTheRemove(item.Name, item.Amount);
    }

    public void RemoveItem(string name, int amount)
    {
        DoTheRemove(name, amount);
    }

    private void DoTheAdd(string name, int amount)
    {
        foreach (var i in Items)
        {
            if (i.Name == name)
            {
                i.Amount += amount;
                return;
            }
        }
        Items.Add(new InventoryItem { Name = name, Amount = amount });
    }

    private void DoTheRemove(string name, int amount)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Name == name)
            {
                Items[i].Amount -= amount;
                if (Items[i].Amount <= 0)
                {
                    Items.RemoveAt(i);
                }
                return;
            }
        }
    }
}