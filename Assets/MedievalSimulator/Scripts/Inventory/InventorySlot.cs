using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

// Класс для управления отдельным слотом инвентаря
public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;      // Иконка предмета

    private Item item;      // Хранимый предмет
    private InventoryUI uiManager; // Ссылка на менеджер UI

    // Метод инициализации (вызывается из InventoryUI при создании слота)
    public void Setup(InventoryUI manager)
    {
        uiManager = manager;
    }

    // Метод для добавления предмета в слот
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    // Метод для очистки слота
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // Метод для использования предмета
    public void UseItem()
    {
        if (item != null)
        {
            item.Use(); // Вызываем действие предмета
        }
    }

    // Обработка клика по слоту (интерфейс IPointerClickHandler)
    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiManager != null)
        {
            uiManager.SelectSlot(this);
        }
    }

    // Вспомогательные методы для InventoryUI
    public bool HasItem() => item != null;
    public Item GetItem() => item;
}
