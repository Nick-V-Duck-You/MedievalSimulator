using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void GetDamage(int damageValue)
    {
        this.gameObject.GetComponent<EnemyStats>().hp -= damageValue;
        if (this.gameObject.GetComponent<EnemyStats>().hp <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(this.gameObject);
    }
}
