using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToLine : MonoBehaviour
{
    public float yOffset = 0.0f;
    public float minY = -5f;
    public float maxY = 3f;
    public string hookTag = "Hook";
    public string attachableTag = "Attachable";
    public AudioClip attachSound;  // Audio clip for the attachment sound
    public AudioClip negativeSound; // Audio clip for the negative feedback sound
    public AudioClip goodObjectSound; // Audio clip for the good object sound
    public int pointsDeductedForBadObject = 10; // Points deducted for bad objects

    private BoxCollider2D boxCollider;
    private bool isObjectAttached = false;
    private Transform attachedObject = null;  // Keep track of the attached object
    private ScoreManager scoreManager;  // Reference to the ScoreManager
    private AudioSource audioSource;  // Reference to AudioSource for playing sounds

    void Start()
    {
        // Ensure that the BoxCollider2D is attached to the sprite
        boxCollider = GetComponent<BoxCollider2D>();

        // Find the ScoreManager in the scene
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }

        // Add an AudioSource component if one does not already exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Control the sprite movement along the Y-axis based on the mouse position
        MoveSpriteWithMouse();

        // Check for mouse click to attach or detach objects
        if (Input.GetMouseButtonDown(0))
        {
            if (!isObjectAttached)
            {
                AttachObjectsInCollisionBox();
            }
            else
            {
                DetachAndDestroyObject();
            }
        }
    }

    void MoveSpriteWithMouse()
    {
        // Get the current mouse Y position and convert it from screen space to world space
        float mouseY = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).y;

        // Clamp the Y position within the specified range and apply the offset
        float clampedY = Mathf.Clamp(mouseY + yOffset, minY, maxY);

        // Update the sprite's position to follow the mouse on the Y-axis only
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    void AttachObjectsInCollisionBox()
    {
        // Check if there are any colliders within the BoxCollider2D when the left mouse button is clicked
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(attachableTag))
            {
                ObjectPoints objectPoints = collider.GetComponent<ObjectPoints>();
                if (objectPoints != null)
                {
                    if (objectPoints.points < 0) // Bad object
                    {
                        // Play negative sound and handle bad object
                        if (negativeSound != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(negativeSound);
                        }
                        // Deduct points for bad object
                        scoreManager.AddPoints(pointsDeductedForBadObject);
                        Destroy(collider.gameObject); // Destroy bad object immediately
                        Debug.Log($"Bad object {collider.name} clicked, points deducted and object destroyed.");
                    }
                    else // Good object
                    {
                        if (!isObjectAttached)
                        {
                            // Attach the object by making it a child of the hook (this object)
                            collider.transform.SetParent(transform);
                            attachedObject = collider.transform; // Keep track of the attached object

                            ObjectMovement objectMovement = collider.GetComponent<ObjectMovement>();
                            if (objectMovement != null)
                            {
                                objectMovement.AttachToHook(transform);
                            }

                            // Play the attachment sound
                            if (attachSound != null && audioSource != null)
                            {
                                audioSource.PlayOneShot(attachSound);
                            }

                            // Mark the object as attached
                            isObjectAttached = true;

                            Debug.Log($"{collider.name} attached to {gameObject.name}");
                            break; // Exit after attaching the first object
                        }
                    }
                }
            }
        }
    }

    void DetachAndDestroyObject()
    {
        if (attachedObject != null)
        {
            // Get the ObjectPoints component to determine the object's point value
            ObjectPoints objectPoints = attachedObject.GetComponent<ObjectPoints>();
            if (objectPoints != null)
            {
                if (objectPoints.points > 0)
                {
                    // Add the object's points to the score
                    scoreManager.AddPoints(objectPoints.points);
                    
                    // Play the good object sound
                    if (goodObjectSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(goodObjectSound);
                    }

                    Debug.Log($"Added {objectPoints.points} points to the score.");
                }

                // Destroy the attached object
                Destroy(attachedObject.gameObject);
            }

            // Reset attachedObject and isObjectAttached flag
            attachedObject = null;
            isObjectAttached = false;

            Debug.Log("Object detached and destroyed");
        }
        else
        {
            Debug.LogWarning("No object attached to destroy.");
        }
    }
}
