using System.Diagnostics;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    PlayerController player;
    private bool isDamage;

    public float damageMultiplier = 1f;
    public float minVelocityToDamage = 2f;
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

 
    void Update()
    {
        if (isDamage == true)
        {
 
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed > minVelocityToDamage)
        {
            float damage = impactSpeed * damageMultiplier;
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                UnityEngine.Debug.Log("Enemy took " + damage + " damage from impact.");
            }
        }
    }

}
