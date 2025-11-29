using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
	public Image healthBar;
	public Image hungerBar;
	public Image inebriationBar;
	
	public PlayerStats player;

	void Start()
    {
		healthBar = healthBar.GetComponent<Image>();
		hungerBar = hungerBar.GetComponent<Image>();
		inebriationBar = inebriationBar.GetComponent<Image>();
		player = FindFirstObjectByType<PlayerStats>();
        healthBar.fillAmount = player.HP / player.maxHP;
    }

    void Update()
    {
		hungerBar.fillAmount = player.hunger / player.maxHunger;
		inebriationBar.fillAmount = player.inebriation / player.maxInebriation;

	}

	public void HealthChange()
	{
        healthBar.fillAmount = player.HP / player.maxHP;
    }
}
