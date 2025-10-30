using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
	public Image healthBar;
	public PlayerStats player;
    public GameObject GPlayer;

	void Start()
    {
        GPlayer = GameObject.FindGameObjectWithTag("Player");
        GPlayer.GetComponent<PlayerStats>().ChangeStatValue("HP",-50);
    }

    void Update()
    {
        
    }
}
