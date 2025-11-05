using UnityEngine;

public class ItemTaker : MonoBehaviour
{
    [SerializeField] Item itemToAdd;
    [SerializeField] Inventory targetInventory;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            targetInventory.AddItem(itemToAdd);
        if (Input.GetKeyUp(KeyCode.R))
            targetInventory.RemoveItem(itemToAdd);
    }
}
