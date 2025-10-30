using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float HP = 74f;
    public float maxHP = 100f;
    
    //variable denoting the time it takes for the indicator to decrease its value by 1f
    public float waitTime = 1.0f;

    public float hunger = 100f;
    public float maxHunger = 100f;

    public float inebriation = 100f;
    public float maxInebriation = 100f;

    // Нужно ли оставлять апдейт в плеер статс?
    void Update()
    {
        hunger-= 1.0f/waitTime * Time.deltaTime;
        inebriation-= 1.0f/waitTime * Time.deltaTime;
    }

    /// <summary>
    /// Changes player stat to the specified value, to decrease must use a negative value
    /// </summary>
    ///<param name ="stat"> Stat name </param>
    ///<param name ="value"> Value to change </param>

    public void ChangeStatValue(string stat, float value)
    {
        switch (stat)
        {
            case "HP":
                HP += value;
                break;
            case "maxHP":
                maxHP += value;
                break;
            case "hunger":
                hunger += value;
                break;
            case "maxHunger":
                maxHunger += value;
                break;
            case "inebriation":
                inebriation += value;
                break;
            case "maxInebriation":
                maxInebriation += value;
                break;
        }
        
    } 
}
