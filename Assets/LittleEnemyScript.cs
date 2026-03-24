using Unity.VisualScripting;
using UnityEngine;

public class LittleEnemyScript : MonoBehaviour
{
    [Header("Rango")]
    [SerializeField] private float followRange = 5f;
    [SerializeField] private float attackRange = 1f;
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 2f;
    [Header("Ataque")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject[] targets;
    private int currentPoint = 0;

    void Update()
    {
        if(player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= followRange)
        {
            FollowPlayer();

        }
        else
        {
            Wondering();
        }
    }

    private void Wondering()
    {
        if (targets.Length == 0) return;

        Transform targetPoint = targets[currentPoint].transform;

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.3f)
        {
            currentPoint++;
            if (currentPoint >= targets.Length)
            {
                currentPoint = 0;
            }
        }
    }
    private void FollowPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void AttackPlayer()
    {
        if (Time.time >= attackCooldown)
        {
            // Implement attack logic here, e.g., reduce player's health
            Debug.Log("Attacking player for " + attackDamage + " damage!");
            attackCooldown = Time.time + 1f; // Reset cooldown
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
