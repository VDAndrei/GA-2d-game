using UnityEngine;

public class CameraShiftTrigger : MonoBehaviour
{
    public float shiftAmount = 36f; // Amount to shift the camera by
    public Vector2 shiftDirection = Vector2.right; // Direction to shift
    public float transitionDuration = 1f; // Duration of the transition

    private Vector3 targetCameraPosition; // The target position for the camera
    private bool isTransitioning = false; // Flag to indicate if a transition is in progress
    private float transitionProgress = 0f; // Progress of the transition

    public GameObject Player;
    private PlayerMovementScript movementScript;

    private void Start()
    {
        movementScript = Player.GetComponent<PlayerMovementScript>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the trigger
        if (other.CompareTag("Player"))
        {
            // Calculate the new target camera position
            Vector3 cameraPosition = Camera.main.transform.position;
            targetCameraPosition = cameraPosition + (Vector3)(shiftDirection * shiftAmount);

            // Start the transition
            isTransitioning = true;
            transitionProgress = 0f; // Reset progress

            movementScript.inTransition = true;
        }
    }

    private void Update()
    {
        // Check if we are transitioning
        if (isTransitioning)
        {
            // Increment the transition progress
            transitionProgress += Time.deltaTime / transitionDuration;

            // Lerp the camera position
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetCameraPosition, transitionProgress);

            // Check if the transition is complete
            if (transitionProgress >= 1f)
            {
                isTransitioning = false; // End the transition
            }
        }
    }
}