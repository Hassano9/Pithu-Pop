using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMovementFree : MonoBehaviour
{
    public float dragSpeed = 2f;  // slow movement
    public float groundY = 1f;       // character height

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            lastTouchPosition = Input.mousePosition;
            MoveCharacter(delta);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                lastTouchPosition = touch.position;
                MoveCharacter(delta);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
#endif
    }

    void MoveCharacter(Vector2 delta)
    {
        // Convert screen drag to world movement
        Vector3 move = mainCam.transform.right * delta.x + mainCam.transform.forward * delta.y;
        move.y = 0;

        // Apply movement
        Vector3 newPos = transform.position + move * dragSpeed;

        // Keep on ground
        newPos.y = groundY;

        transform.position = newPos;
    }
}
