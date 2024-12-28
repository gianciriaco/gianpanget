using System.Collections; // Add this for IEnumerator
using UnityEngine;

public class Player1 : MonoBehaviour
{
    public float moveSpeed = 2f; // Movement speed of the player
    public float attackRange = 1.5f; // Range at which the player will attack the enemy
    public int damage = 1; // Damage dealt to the enemy

    private Transform enemy;
    private Vector3 startPosition;
    private bool isAttacking = false; // Prevents overlapping actions
    private Animator animator; // Reference to the Animator component

    public int maxHealth = 3; // Maximum health of the player
    private int currentHealth;

    void Start()
    {
        // Initialize current health
        currentHealth = maxHealth;

        // Find the enemy in the scene (ensure Enemy1 tag is set on the enemy)
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform;
        startPosition = transform.position;

        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return; // Prevent movement while attacking

        // Ensure that the enemy still exists
        if (enemy == null) return;

        // Listen for the "Y" key press to start the sequence
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(MoveAndAttackSequence());
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject); // Destroy the player object when health reaches 0
    }

    void MoveTowardsEnemy()
    {
        // Lock the y-axis to prevent upward movement
        Vector3 targetPosition = new Vector3(enemy.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Trigger movement animation
        animator.SetBool("isMoving", true);
    }

    void StartAttack()
    {
        // Stop movement and trigger attack logic
        animator.SetBool("isMoving", false); // Stop moving animation
        animator.SetBool("isAttacking", true); // Start attack animation
        StartCoroutine(AttackAndRetreat());
    }

    IEnumerator AttackAndRetreat()
    {
        isAttacking = true;

        // Wait for the attack animation to complete (adjust the duration to match your attack animation)
        yield return new WaitForSeconds(0.5f);

        // Damage the enemy if enemy exists
        if (enemy != null)
        {
            Enemy1 enemyScript = enemy.GetComponent<Enemy1>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage); // Reduce enemy health
            }
        }

        // Stop attack animation
        animator.SetBool("isAttacking", false);

        // Smoothly move back to the starting position
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the character is in the idle state after retreating
        animator.SetBool("isMoving", false);
        isAttacking = false;
    }

    // Method to take damage (can be used to reduce the player's health)
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method if health reaches 0
        }
    }

    // Coroutine to handle both movement and attack when pressing "Y"
    IEnumerator MoveAndAttackSequence()
    {
        // Move towards the enemy
        while (Vector3.Distance(transform.position, enemy.position) > attackRange)
        {
            MoveTowardsEnemy();
            yield return null; // Wait for the next frame
        }

        // Once within attack range, trigger the attack
        StartAttack();
    }
}
