using System.Collections.Generic;
using UnityEngine;

// Класс для обновления UI инвентаря
public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Control")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject removeButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;

    [Header("Inventory UI")]
    public Transform itemsParent;
    public GameObject slotPrefab;
    public Inventory inventory;

    private List<InventorySlot> slots = new List<InventorySlot>(); // Список слотов
    private bool inventoryOpen = false;

    void Start()
    {
        if (inventory != null)
            inventory.onItemChangedCallback += UpdateUI;

        UpdateUI();
        SetInventoryState(false);
    }

    // Метод для обновления интерфейса
    void Update()
    {
        // Проверяем нажатие клавиши "Инвентаря"
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }

        // Проверяем нажатие клавиши E
        if (Input.GetKeyUp(KeyCode.E))
        {
            AddSlot(); // Добавляем новый слот
        }
    }

    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        SetInventoryState(inventoryOpen);
    }

    public void OpenInventory()
    {
        inventoryOpen = true;
        SetInventoryState(true);
    }

    public void CloseInventory()
    {
        inventoryOpen = false;
        SetInventoryState(false);
    }

    // Включение и выключение панели + управление временем и героем
    private void SetInventoryState(bool isOpen)
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        if (removeButton != null)
            removeButton.SetActive(isOpen);


        if (isOpen)
        {
            // UI режим
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Останавливаем время
            Time.timeScale = 0f;

            // Отключаем управление персонажем
            if (playerController != null)
                playerController.SetActive(false);
        }
        else
        {
            // Возвращаем игровой режим
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Возвращаем время
            Time.timeScale = 1f;

            // Включаем управление персонажем
            if (playerController != null)
                playerController.SetActive(true);
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