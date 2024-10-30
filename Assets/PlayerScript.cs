using System.Collections;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.0f; // Default speed
    [SerializeField] private float attackResetTime = 1.0f; // Time to reset attack sequence
    [SerializeField] private float attackCooldown = 0.4f; // Time between each attack animation
    [SerializeField] private float heavyAttackCooldown = 1.0f; // Cooldown after third attack

    private Vector2 movement;
    private Animator animator;
    private int attackIndex = 0; // Tracks which attack animation to play
    private float lastAttackTime = 0f; // Time since last attack
    private float lastKeyPressedTime = -Mathf.Infinity; // Time of the last L key press
    private bool isInHeavyAttackCooldown = false; // Flag to check if in heavy attack cooldown
    private bool canAttack = true; // Flag to determine if attack can be performed

    public bool inTransition = false;

    // Array to store the duration of each attack animation
    private float[] animationDurations = { 0.433f, 0.433f, 0.433f };

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check for sprint input and adjust speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 6.0f; // Sprint speed
        }
        else
        {
            movementSpeed = 5.0f; // Normal speed
        }

        // Get raw input for both horizontal and vertical axes
        if (!inTransition)
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
        // Set the animator's speed parameter based on movement
        float speed = movement.magnitude * movementSpeed;
        animator.SetFloat("Speed", speed);

        // Handle flipping based on horizontal movement
        if (movement.x != 0)
        {
            bool flipped = movement.x < 0;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
        }

        // Handle attack input with cooldown
        if (Input.GetKeyDown(KeyCode.L) && canAttack)
        {
            if (Time.time >= lastKeyPressedTime + (isInHeavyAttackCooldown ? heavyAttackCooldown : attackCooldown))
            {
                Attack();
                lastKeyPressedTime = Time.time; // Update the time of the last key press
            }
        }

        // Reset attack combo if time passed since last attack exceeds the reset time
        if (Time.time - lastAttackTime > attackResetTime)
        {
            attackIndex = 0;
            if (isInHeavyAttackCooldown)
            {
                canAttack = true; // Re-enable attack after heavy attack cooldown
                isInHeavyAttackCooldown = false; // Reset heavy attack cooldown flag
            }
        }
    }

    private void Attack()
    {
        // Update last attack time
        lastAttackTime = Time.time;

        // Determine which attack animation to play based on attackIndex
        if (attackIndex == 0)
        {
            animator.SetTrigger("Attack1Trigger");
        }
        else if (attackIndex == 1)
        {
            animator.SetTrigger("Attack2Trigger");
        }
        else if (attackIndex == 2)
        {
            animator.SetTrigger("Attack3Trigger");
            isInHeavyAttackCooldown = true; // Start heavy attack cooldown
            canAttack = false; // Disable attack until cooldown period is over
        }

        // Increment attackIndex and reset if it exceeds the number of attack animations
        attackIndex = (attackIndex + 1) % 3;

        // Set IsAttacking to true and start coroutine to reset it
        animator.SetBool("IsAttacking", true);

        // Use the duration specific to the current attack animation
        float currentAnimationDuration = animationDurations[attackIndex];
        StartCoroutine(ResetAttackFlag(currentAnimationDuration));
    }

    private IEnumerator ResetAttackFlag(float duration)
    {
        // Wait for the duration of the current attack animation
        yield return new WaitForSeconds(duration);

        // Ensure IsAttacking is set to false after the animation has played
        animator.SetBool("IsAttacking", false);
    }

    private void FixedUpdate()
    {
        // Apply the movement
        Vector3 moveDelta = new Vector3(movement.x, movement.y, 0) * movementSpeed * Time.fixedDeltaTime;
        this.transform.Translate(moveDelta, Space.World);
    }
}
