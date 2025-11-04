using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string Name = "Item";
    public Sprite Icon;
    public int price;
    
}
