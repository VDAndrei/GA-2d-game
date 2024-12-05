using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircleDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 5f; // The radius of the detection circle for player and breadcrumbs
    public LayerMask playerLayer;      // Layer for detecting the player
    public LayerMask breadcrumbLayer;  // Layer for detecting breadcrumbs

    [Header("Behavior Settings")]
    public float moveSpeed = 2f; // Speed at which the enemy moves towards the target

    private Transform player;   // Reference to the player's transform
    private bool playerInRange; // Whether the player is within the detection radius

    void Update()
    {
        CheckForPlayer();

        if (playerInRange)
        {
            FollowPlayer();
        }
        else
        {
            FollowNearestBreadcrumb();
        }
    }

    void CheckForPlayer()
    {
        // Check if the player is inside the detection radius
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            player = hit.transform; // Get the player's transform
            playerInRange = true;
        }
        else
        {
            player = null;
            playerInRange = false;
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        // Move the enemy towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void FollowNearestBreadcrumb()
    {
        // Find all breadcrumbs within the detection radius
        Collider2D[] breadcrumbsInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, breadcrumbLayer);

        if (breadcrumbsInRange.Length == 0) return; // No breadcrumbs in range

        // Find the nearest breadcrumb
        Transform nearestBreadcrumb = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D breadcrumb in breadcrumbsInRange)
        {
            float distance = Vector2.Distance(transform.position, breadcrumb.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestBreadcrumb = breadcrumb.transform;
            }
        }

        if (nearestBreadcrumb == null) return;

        // Move towards the nearest breadcrumb
        Vector2 direction = (nearestBreadcrumb.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, nearestBreadcrumb.position, moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Editor for visualization
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}