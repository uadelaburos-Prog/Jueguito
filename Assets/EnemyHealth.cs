using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    private float currentHealth;
    void Start()
    {
        currentHealth = health;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
