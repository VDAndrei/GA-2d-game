using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breadcrumb : MonoBehaviour
{
    [Header("Breadcrumb Settings")]
    public GameObject breadcrumbPrefab; // The prefab for the breadcrumb
    public float breadcrumbLifetime = 5f; // Time before breadcrumbs disappear
    public float breadcrumbInterval = 1f; // Time interval between breadcrumb drops

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Drop a breadcrumb every 'breadcrumbInterval' seconds
        if (timer >= breadcrumbInterval)
        {
            DropBreadcrumb();
            timer = 0f;
        }
    }

    void DropBreadcrumb()
    {
        if (breadcrumbPrefab != null)
        {
            // Instantiate the breadcrumb at the player's position
            GameObject breadcrumb = Instantiate(breadcrumbPrefab, transform.position, Quaternion.identity);

            // Destroy the breadcrumb after 'breadcrumbLifetime'
            Destroy(breadcrumb, breadcrumbLifetime);
        }
    }
}