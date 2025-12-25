using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Control")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button useButton; 
    [SerializeField] private Button removeButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private KeyCode inventoryKey = KeyCode.I;

    [Header("Inventory UI")]
    public Transform itemsParent; //Content в Scroll View
    public GameObject slotPrefab;
    public Inventory inventory; //Объект на котором висит скрипт инвентарь

    private List<InventorySlot> slots = new List<InventorySlot>(); //Список активных слотов
    private bool inventoryOpen = false;

    public InventorySlot SelectedSlot { get; private set; }
    public int SelectedIndex { get; private set; } = -1; 
    
    [SerializeField] private Color selectedColor = new Color(1f, 0.2f, 0.2f, 0.9f); 
    private Color defaultColor; 

    void Start()
    {
        if (inventory != null)
            inventory.onItemChangedCallback += UpdateUI;

        if (slotPrefab != null)
        {
            Image slotImage = slotPrefab.GetComponent<Image>();
            if (slotImage != null) defaultColor = slotImage.color;
        }

        if (useButton != null) useButton.onClick.AddListener(OnUseSelectedButton);
        if (removeButton != null) removeButton.onClick.AddListener(OnRemoveSelectedButton);
        
        if (useButton != null) useButton.interactable = false;
        if (removeButton != null) removeButton.interactable = false;

        UpdateUI();
        SetInventoryState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }
    }

    public void OnUseSelectedButton()
    {
        // Проверяем что слот выбран, индекс корректен и в слоте есть предмет
        if (SelectedSlot != null && SelectedIndex != -1 && SelectedSlot.HasItem())
        {
            SelectedSlot.UseItem();
            // После использования предмета сразу удаляем по индексу через Inventory Manager
            Inventory.instance.RemoveAt(SelectedIndex);
        }
    }

    public void OnRemoveSelectedButton()
    {
        if (SelectedSlot != null && SelectedIndex != -1 && SelectedSlot.HasItem())
        {
            // удаляем по индексу через Inventory Manager
            Inventory.instance.RemoveAt(SelectedIndex);
        }
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (SelectedSlot != null)
        {
            SelectedSlot.GetComponent<Image>().color = defaultColor;
        }

        SelectedSlot = slot;

        SelectedIndex = slots.IndexOf(SelectedSlot);

        if (SelectedSlot != null)
        {
            SelectedSlot.GetComponent<Image>().color = selectedColor;
        }

        bool hasItem = (SelectedSlot != null && SelectedSlot.HasItem());
        if (useButton != null) useButton.interactable = hasItem;
        if (removeButton != null) removeButton.interactable = hasItem;
    }

    public void DeselectSlot()
    {
        SelectSlot(null);
        SelectedIndex = -1; // Сбрасываем индекс при снятии выделения
    }


    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        SetInventoryState(inventoryOpen);
        if (!inventoryOpen)
        {
            DeselectSlot();
        }
    }

    public void OpenInventory() => SetInventoryState(true);
    public void CloseInventory() => SetInventoryState(false);


    private void SetInventoryState(bool isOpen)
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        // Внешние кнопки показываем только когда инвентарь открыт
        if (useButton != null) useButton.gameObject.SetActive(isOpen);
        if (removeButton != null) removeButton.gameObject.SetActive(isOpen);


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

    void CreateNewSlotForNewItem() 
    {
        var go = Instantiate(slotPrefab, itemsParent);
        var slot = go.GetComponent<InventorySlot>();
        slot.Setup(this); 
        slots.Add(slot);
    }

    void RemoveLastSlot()
    {
        if (slots.Count == 0) return;

        InventorySlot slotToRemove = slots[slots.Count - 1];
        slots.RemoveAt(slots.Count - 1);
        Destroy(slotToRemove.gameObject);
    }


    // Синхронизация UI со списком предметов
    void UpdateUI()
    {
        
        while (slots.Count < inventory.items.Count)
        {
            CreateNewSlotForNewItem();
        }

        while (slots.Count > inventory.items.Count)
        {
            RemoveLastSlot();
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.items.Count)
                slots[i].AddItem(inventory.items[i]);
        }
        
        if (SelectedSlot != null && !SelectedSlot.HasItem())
        {
            DeselectSlot(); // Снять выделение, если предмет только что был удален из выбранного слота
        }
    }
}
