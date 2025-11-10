using System.Collections.Generic;
using UnityEngine;

// Класс для обновления UI инвентаря
public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent; // Родитель слотов
    public GameObject slotPrefab;
    public Inventory inventory;

    private List<InventorySlot> slots = new List<InventorySlot>(); // Массив слотов

    void Start()
    {
        inventory.onItemChangedCallback += UpdateUI;
        UpdateUI();
    }

    // Метод для обновления интерфейса
    void Update()
    {
        // Проверяем нажатие клавиши E
        if (Input.GetKeyUp(KeyCode.E))
        {
            AddSlot(); // Добавляем новый слот
        }
    }
    // Создание нового слота
    void AddSlot()
    {
        if (slots.Count >= inventory.space) return;

        var go = Instantiate(slotPrefab, itemsParent);
        var slot = go.GetComponent<InventorySlot>();
        slot.ClearSlot();
        slots.Add(slot);
    }

    // Обновление UI
    void RemoveLastSlot()
    {
        if (slots.Count == 0) return;

        InventorySlot slot = slots[slots.Count - 1];
        slots.RemoveAt(slots.Count - 1);
        Destroy(slot.gameObject); // Удаляем сам объект из UI
        UpdateUI();
    }

    // Синхронизация UI со списком предметов
    void UpdateUI()
    {
        // Гарантируем, что количество слотов не меньше количества предметов
        while (slots.Count < inventory.items.Count)
            AddSlot();

        // Лишние слоты — удаляем
        while (slots.Count > inventory.items.Count)
            RemoveLastSlot();

        // Обновляем каждый слот
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.items.Count)
                slots[i].AddItem(inventory.items[i]);
            else
                slots[i].ClearSlot();
        }
    }
}