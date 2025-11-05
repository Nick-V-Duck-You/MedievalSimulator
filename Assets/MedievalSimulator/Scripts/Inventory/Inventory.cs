using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Inventory : MonoBehaviour
{
    public Action<Item> onItemAdded;
    public Action<Item> onItemRemoved;

    [FormerlySerializedAs ("StartItems")] [SerializeField] public List<Item> StartItems = new List<Item>();

    public List<Item> inventoryItems { get; set; }
    

    void Awake()
    {
        inventoryItems = new List<Item>();
        for (var i = 0; i < StartItems.Count; i++) 
        {
            AddItem(StartItems[i]);
        
        }

    }


    public void AddItem(Item item)
    {
        inventoryItems.Add(item);

        onItemAdded?.Invoke(item);
    }
    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);

        onItemAdded?.Invoke(item);
    }
}
