using UnityEngine;

// Класс для обновления UI инвентаря
public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent; // Родитель слотов
    public Inventory inventory;

    public InventorySlot[] slots; // Массив слотов

    void Start()
    {
        inventory.onItemChangedCallback += UpdateUI; // Подписываемся на изменения инвентаря
        slots = itemsParent.GetComponentsInChildren<InventorySlot>(); // Получаем все слоты
    }

    // Метод для обновления интерфейса
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]); // Заполняем слот
            }
            else
            {
                slots[i].ClearSlot(); // Очищаем слот
            }
        }
    }
}