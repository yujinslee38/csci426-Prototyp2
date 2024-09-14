using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object moves
    public float fadeDuration = 1f; // Time to fade out

    private SpriteRenderer spriteRenderer;
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
        // Move the object to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Check if the object has reached the left edge of the screen
        if (transform.position.x <= screenLeftEdge)
        {
            // Start fading out
            StartCoroutine(FadeOut());
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