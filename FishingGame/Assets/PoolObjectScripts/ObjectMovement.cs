using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float speed = 5f; // Speed at which the object moves
    public float fadeDuration = 1f; // Time to fade out
    private SpriteRenderer spriteRenderer;
    public bool isAttached;
    private Transform hookTransform;
    private Coroutine fadeCoroutine;
    private Collider2D objectCollider;
    public string attachableTag = "Attachable";
    private float screenLeftEdge;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAttached = false;
        screenLeftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    }

    void Update()
    {
        if (!isAttached)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (transform.position.x <= screenLeftEdge)
            {
                if (fadeCoroutine == null)
                {
                    fadeCoroutine = StartCoroutine(FadeOut());
                }
            }
        }
        else
        {
            transform.position = hookTransform.position;
        }
    }

    public void AttachToHook(Transform hook)
    {
        if (GetComponent<ObjectPoints>().points < 0) // Check if it's a bad object
        {
            Debug.Log("Cannot attach bad object.");
            return; // Do not attach if it's a bad object
        }

        isAttached = true;
        hookTransform = hook;
        speed = 0f; // Stop movement
        transform.SetParent(hook);

        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
