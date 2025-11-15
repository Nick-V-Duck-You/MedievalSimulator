using UnityEngine;

public class ItemTaker : MonoBehaviour
{
    [SerializeField] Item itemToAdd;
    [SerializeField] Inventory targetInventory;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
            targetInventory.Add(itemToAdd);
        if (Input.GetKeyUp(KeyCode.R))
            targetInventory.Remove(itemToAdd);
    }

    public void Remove()
    {
        targetInventory.Remove(itemToAdd);
    }
}
