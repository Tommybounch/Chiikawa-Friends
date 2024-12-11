using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;
    private Dictionary<CollectableType, Collectable> collectableItemsDict = new Dictionary<CollectableType, Collectable>();

    private void Awake()
    {
        foreach(Collectable item in collectableItems)
        {
            AddItem(item);

        }
    }

    private void AddItem(Collectable item)
    {
        if(!collectableItemsDict.ContainsKey(item.type))
        {
            collectableItemsDict.Add(item.type, item);
        }
    }

    public Collectable GetItemByType(CollectableType type)
    {
        if(collectableItemsDict.ContainsKey(type))
        {
            return collectableItemsDict[type];
        }

        return null;
    }
}
