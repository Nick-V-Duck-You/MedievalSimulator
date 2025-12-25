using System.Collections.Generic;
using UnityEngine;

// Класс инвентаря для управления предметами
public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Синглтон для удобного доступа

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Inventory уже существует!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public List<Item> items = new List<Item>(); // Список предметов в инвентаре
    public int space = 20; // Максимальное количество предметов

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    // Метод для добавления предмета
    public bool Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Нет места в инвентаре!");
            return false;
        }

        items.Add(item);
        onItemChangedCallback?.Invoke(); // Уведомляем об изменениях
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke(); // Уведомляем об изменениях
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
            onItemChangedCallback?.Invoke(); // Уведомляем об изменениях
        }
    }
}
