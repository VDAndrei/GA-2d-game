using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    public Image healthBar;

    private Animator animator;
    private float previousHealth;
    private bool isUpdatingHealthBar = false;
    private bool isDead = false; // Flag to track if player is dead
    private PlayerMovementScript playerMovement;

    void Start()
    {
        maxHealth = health;
        previousHealth = health;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        if (isDead) return; // Stop further updates if the player is dead

        // Check if health has decreased
        if (health < previousHealth)
        {
            // Force play the damage animation immediately
            animator.CrossFade("Hitanimation", 0.1f);

            // Start smooth health bar update
            if (!isUpdatingHealthBar)
            {
                StartCoroutine(UpdateHealthBarSmoothly(previousHealth, health));
            }

            previousHealth = health; // Update previous health
        }

        // Check if health reaches zero to play the death animation
        if (health <= 0)
        {
            healthBar.fillAmount = 0;
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Set the player as dead to prevent further updates
        Debug.Log("Player is dying, playing death animation.");
        animator.SetTrigger("Die");

        // Disable movement and any other necessary components
        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable PlayerMovementScript
        }
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        // Use layer index 0 for the base layer
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    IEnumerator UpdateHealthBarSmoothly(float startHealth, float targetHealth)
    {
        isUpdatingHealthBar = true;
        float elapsed = 0f;
        float duration = 0.3f; // Adjust for faster or slower health bar decrease

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentHealth = Mathf.Lerp(startHealth, targetHealth, elapsed / duration);
            healthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
            yield return null;
        }

        // Finalize the health bar position to match target health
        healthBar.fillAmount = Mathf.Clamp01(targetHealth / maxHealth);

        if (health <= 0) // Ensure health bar is empty if health is zero
        {
            healthBar.fillAmount = 0;
        }

        isUpdatingHealthBar = false;
    }
}
