using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header(" --- Zooming ---")]
    [Tooltip("The closest to the game plane the camera is allowed to be.")]
    [SerializeField] private float minDist;
    [Tooltip("The farthest from the game plane the camera is allowed to be.")]
    [SerializeField] private float maxDist;
    [Tooltip("How far the camera moves per 'unit' of zoom.")]
    [SerializeField] private float deltaDist;
    private KeyCode keyZoomIn = KeyCode.Equals;
    private KeyCode keyZoomOut = KeyCode.Minus;

    private Vector2 prevMousePos = Vector2.zero;
    private bool panningMode = false;

    private Camera myCam;

    private void Start()
    {
        myCam = GetComponent<Camera>();
    }

    private void Update()
    {
        ResolveZoom();
        ResolvePan();
    }

    /**
     * Manages Camera Control related to zooming.
     */
    private void ResolveZoom()
    {
        // --- Camera Zoom ---
        // Account for zoom keybinds
        // TODO: Repeat action when key(s) held
        if (Input.GetKeyDown(keyZoomIn))
            transform.position += Vector3.forward * deltaDist;
        if (Input.GetKeyDown(keyZoomOut))
            transform.position -= Vector3.forward * deltaDist;
        // Account for scroll wheel
        transform.position += Vector3.forward * deltaDist * Input.mouseScrollDelta.y;
        // Clamp results
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -maxDist, -minDist));
    }

    /**
     * Manages Camera Control related to panning.
     */
    private void ResolvePan()
    {
        // Cache mouse position for next frame.
        Vector2 cachedPos = prevMousePos;
        prevMousePos = Input.mousePosition;

        // --- Camera Pan ---
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonUp(2))
        {
            // Saving an IF statement here by assuming that ButtonDown() and ButtonUp() will never both be true.
            panningMode = Input.GetMouseButtonDown(2);
            // We don't want to start moving the camera while the cached position was from a frame where the button wasn't held.
            return;
        }

        if (panningMode)
        {
            // Determine the distance in pixels the mouse has moved since last frame.
            Vector2 posDelta = prevMousePos - cachedPos;

            // Convert the position delta from absolute pixel distances to fractions of the total screen height/width.
            posDelta = new Vector2(posDelta.x / Screen.width, posDelta.y / Screen.height);

            // Calculate the dimensions of the visible area.
            float frustumHeight = 2.0f * -transform.position.z * Mathf.Tan(myCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * myCam.aspect;

            // Apply the relative position delta in screen coordinates to the visible area to get world coordinates.
            Vector2 posDeltaWorld = new Vector2(posDelta.x * frustumWidth, posDelta.y * frustumHeight);

            transform.position -= new Vector3(posDeltaWorld.x, posDeltaWorld.y, 0);
        }
    }
}
