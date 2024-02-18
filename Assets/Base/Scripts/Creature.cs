using UnityEngine;

public class Creature : MonoBehaviour
{
    protected int health;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
