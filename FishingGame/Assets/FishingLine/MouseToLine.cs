using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToLine : MonoBehaviour
{
    public float yOffset = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //Get the current mouse y position(convert screen space to world space)
        float mouseY = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).y;

        float clampedY = Mathf.Clamp(mouseY + yOffset, -5f, 3f); //adjust limits if necessary

        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}
