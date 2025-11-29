using UnityEngine;

public class ItemTaker : MonoBehaviour
{
    [SerializeField] Item itemToAdd;
    [SerializeField] public Item itemToRemove;
    [SerializeField] Inventory targetInventory;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            targetInventory.Add(itemToAdd);
        if (Input.GetKeyUp(KeyCode.R))
            targetInventory.Remove(itemToRemove);
    }

    public void Remove()
    {
        targetInventory.Remove(itemToRemove);
    }
}
