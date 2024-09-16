using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseToLine : MonoBehaviour
{
    public float yOffset = 0.0f;
    public float minY = -5f;
    public float maxY = 3f;
    public string hookTag = "Hook";
    public string attachableTag = "Attachable";
    private BoxCollider2D boxCollider;
    private Rigidbody2D attachedRigidbody;
    private bool isObjectAttached = false;

    public void Start()
    {
        // Ensure that the BoxCollider2D is attached to the sprite
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        // Control the sprite movement along the Y-axis based on the mouse position
        MoveSpriteWithMouse();

        // Check for mouse click to attach objects
        if (Input.GetMouseButtonDown(0) && !isObjectAttached)
        {
            AttachObjectsInCollisionBox();
        }

        //add in limit once line is introduced, so when the mouse button down and the object is over the line. 
        else if(Input.GetMouseButtonDown(0) && isObjectAttached)
        {
            Destroy(transform.GetChild(0).gameObject);
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
            if (collider.CompareTag(attachableTag) && !isObjectAttached) // Ensure only one object is attached
            {
                // Attach the object by making it a child of the hook (this object)
                collider.transform.SetParent(transform);
                ObjectMovement objectMovement = collider.GetComponent<ObjectMovement>();
                
                if(objectMovement != null)
                {
                    objectMovement.AttachToHook(transform);
                }

                // Mark the object as attached
                isObjectAttached = true;

                Debug.Log($"{collider.name} attached to {gameObject.name}");
                break; // Exit after attaching the first object
            }
        }
    }
}
