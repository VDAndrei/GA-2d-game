using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    // SerializeField makes this visible and editable in the Unity Inspector
    [SerializeField] private float movementSpeed = 1.0f; // Default speed

    private Vector2 movement;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        // Optional: Change speed during runtime
        movementSpeed = 1.0f;
    }

    private void Update()
    {
        // Check for sprint input and adjust speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = 1.5f; // Sprint speed
        }
        else
        {
            movementSpeed = 1.0f; // Normal speed
        }

        // Get raw input for both horizontal and vertical axes
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Set the animator's speed parameter to the magnitude of the movement
        animator.SetFloat("Speed", movement.magnitude * movementSpeed);

        // Handle flipping based on horizontal movement
        if (movement.x != 0)
        {
            bool flipped = movement.x < 0;
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
        }
    }

    private void FixedUpdate()
    {
        // Apply the movement
        Vector3 moveDelta = new Vector3(movement.x, movement.y, 0) * movementSpeed * Time.fixedDeltaTime;
        this.transform.Translate(moveDelta, Space.World);
    } 
}
