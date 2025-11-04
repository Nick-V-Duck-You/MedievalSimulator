using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory TargetInventory;
    [SerializeField] RectTransform itemsPanel;

    readonly List<GameObject> drawnIcons = new List<GameObject>();

    void Start()
    {
        TargetInventory.onItemAdded += OnItemAdded;
        TargetInventory.onItemRemoved -= OnItemRemoved;
        Redraw();
    }

    void OnItemAdded(Item obj) => Redraw();
    void OnItemRemoved(Item obj) => Redraw();


    // Update is called once per frame
    void Update()
    {

    }
    void Redraw()
    {
        ClearDrawn();
        
        for (var i = 0; i < TargetInventory.inventoryItems.Count; i++)
        {
            var item = TargetInventory.inventoryItems[i];
            var icon = new GameObject("Icon");
            icon.AddComponent<Image>().sprite = item.Icon;
            icon.transform.SetParent(itemsPanel);

            drawnIcons.Add(icon);
        }
    }

    void ClearDrawn()
    {
        for (var i = 0;i < drawnIcons.Count; i++)
        {
            Destroy(drawnIcons[i]);
        }
        drawnIcons.Clear();
    }
}