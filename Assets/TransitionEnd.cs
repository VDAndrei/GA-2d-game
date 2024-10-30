using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnd : MonoBehaviour
{

    public GameObject Player;

    private PlayerMovementScript movementScript;

    public BoxCollider2D Barrier;
    void Start()
    {

        movementScript = Player.GetComponent<PlayerMovementScript>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {

            movementScript.inTransition = false;

            Barrier.enabled = true;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
