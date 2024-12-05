using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadcrumbDropping : MonoBehaviour
{
    [Header("Breadcrumb Settings")]
    public GameObject breadcrumbPrefab; // The prefab for the breadcrumb (empty GameObject)
    public float dropInterval = 2f; // Time interval between breadcrumb drops
    public float breadcrumbLifetime = 5f; // How long breadcrumbs last before despawning

    private void Start()
    {
        // Start dropping breadcrumbs at regular intervals
        StartCoroutine(DropBreadcrumbs());
    }

    private IEnumerator DropBreadcrumbs()
    {
        while (true)
        {
            // Instantiate a breadcrumb at the current position and rotation
            GameObject breadcrumb = Instantiate(breadcrumbPrefab, transform.position, Quaternion.identity);

            // Destroy the breadcrumb after the set lifetime
            Destroy(breadcrumb, breadcrumbLifetime);

            // Wait for the next interval
            yield return new WaitForSeconds(dropInterval);
        }
    }
}