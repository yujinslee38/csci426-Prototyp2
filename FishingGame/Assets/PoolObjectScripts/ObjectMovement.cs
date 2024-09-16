using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object moves
    public float fadeDuration = 1f; // Time to fade out
    private SpriteRenderer spriteRenderer;
    private bool isAttached = false;
    public string attachableTag = "Attachable";
    private Transform hookTransform;
    private Coroutine fadeCoroutine;
    private Collider2D objectCollider;
    public string hookTag = "Hook"; // Changed to hook's tag
    private float screenLeftEdge;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calculate the left edge of the screen
        screenLeftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    void Update()
    {
        if (!isAttached)
        {
            // Move the object leftwards
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Check if the object has reached the left edge of the screen
            if (transform.position.x <= screenLeftEdge)
            {
                // Start fading out if it goes off-screen, if it hasn't already
                if (fadeCoroutine == null)
                {
                    fadeCoroutine = StartCoroutine(FadeOut());
                }
            }
        }
        else if (hookTransform != null)
        {
            // Keep the object attached to the hook by setting it to the hook's position
            transform.position = hookTransform.position;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(hookTag) && !isAttached)
        {
            // Stop the object's movement and attach it to the hook
            isAttached = true;

            // Set the hook as the parent of this object
            hookTransform = other.transform;

            // Optionally, if you want to parent the object to the hook in the hierarchy
            transform.SetParent(hookTransform);

            // Disable further collision detection to avoid re-triggering
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }

            // Stop fading out if the object attaches before reaching the edge
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
        }
    }

    IEnumerator FadeOut()
    {
        // Gradually decrease the alpha value of the sprite's color
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Destroy the object after fading out
        Destroy(gameObject);
    }
}