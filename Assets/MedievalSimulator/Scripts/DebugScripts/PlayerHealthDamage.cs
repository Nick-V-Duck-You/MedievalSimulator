using UnityEngine;

public class PlayerHealthDamage : MonoBehaviour
{


    [SerializeField] private PlayerStatsSounds sounds;
    [SerializeField] private PlayerStatsUI ui;
    [SerializeField] private PlayerStats stats;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            stats.HP -= 10;
            sounds.HealthChange();
            ui.HealthChange();
        }
    }
}
