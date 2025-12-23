using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    [SerializeField] public GameObject Attacker;

    private void Start()
    {
        damage = Attacker.GetComponent<PlayerStats>()?.playerDamage ?? 0;
        //damage = Attacker.GetComponent<AllyStats>()?.AllyDamage ?? 0; - если скрипт будет висеть на оружии спутника
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<EnemyDeath>()?.GetDamage(damage);
    }
}
