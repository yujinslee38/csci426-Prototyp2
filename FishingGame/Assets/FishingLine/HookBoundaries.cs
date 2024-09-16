using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBoundaries : MonoBehaviour
{
    public float minY = -5f; // Minimum Y position (e.g., bottom of the screen)
    public float maxY = 3f;  // Maximum Y position (e.g., top of the screen)

    private void Update()
    {
        // Ensure the hook's position stays within the defined Y boundaries
        Vector3 position = transform.position;

        // Clamp the Y position to ensure it doesn't go beyond the minY and maxY
        position.y = Mathf.Clamp(position.y, minY, maxY);

        // Apply the clamped position back to the transform
        transform.position = position;
    }
}
