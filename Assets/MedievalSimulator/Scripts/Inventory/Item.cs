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

    // Метод, который может быть переопределен для уникальных действий с предметом
    public virtual void Use()
    {
        Debug.Log($"Использован предмет: {itemName}");

        PlayerStats playerStats = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();
        if (playerStats == null) return; 
        
        PlayerStatsSounds sounds = GameObject.FindWithTag("Player")?.GetComponent<PlayerStatsSounds>();
        PlayerStatsUI ui = GameObject.FindWithTag("UIManager")?.GetComponent<PlayerStatsUI>();

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
            if (ui != null) ui.HealthChange();
            if (sounds != null) sounds.HealthChange();
        }

    }
}
