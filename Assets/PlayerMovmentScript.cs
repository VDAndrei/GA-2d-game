using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovmentScript : MonoBehaviour
{
    public float moveSpeed; // Movement speed variable

    private Rigidbody2D rb; // Rigidbody2D component

    private float x; // Horizontal input
    private float y; // Vertical input

    private Vector2 input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Assign the Rigidbody2D component
    }

    private void Update()
    {
        GetInput(); // Call method to get input
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(x * moveSpeed, y * moveSpeed); // Apply velocity
    }

    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        y = Input.GetAxisRaw("Vertical");   // Get vertical input

        input = new Vector2(x, y);
        input.Normalize();
    }
}
