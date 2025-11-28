using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // Название предмета
    public Sprite icon;     // Иконка предмета
    public bool isStackable; // Можно ли складывать в стопки
    public bool isHealer;
    public bool isFood;
    public bool isDrink;

    private PlayerStats playerStats;

    // Метод, который может быть переопределен для уникальных действий с предметом
    public virtual void Use()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        Debug.Log($"Использован предмет: {itemName}");
        if (isDrink)
        {
            playerStats.inebriation += 10;
        }
        if (isFood)
        {
            playerStats.hunger += 10;
        }
        if (isHealer)
        {
            playerStats.HP += 10;
        }
        GameObject.FindWithTag("Player").GetComponent<ItemTaker>().Remove();
    }
}