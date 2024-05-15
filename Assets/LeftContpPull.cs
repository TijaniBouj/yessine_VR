using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftContpPull : MonoBehaviour
{
    public Transform LeftshooterTip;
    private GameObject hitMarker; // Reference to the instantiated hit marker
    public GameObject hitMarkerPrefab; // Prefab for the hit marker
    public LayerMask webLayers;
    public LineRenderer lineRenderer;
    public InputActionProperty pull;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHitMarkerPosition();

        if (pull.action.WasPressedThisFrame())
        {
            // Set the position count to 2 and update the positions
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, LeftshooterTip.position);

            RaycastHit hitInfo;
            if (Physics.Raycast(LeftshooterTip.position, LeftshooterTip.forward, out hitInfo, 1000, webLayers))
            {
                // If the raycast hits an anchor, set the second position to the hit point
                lineRenderer.SetPosition(1, hitInfo.point);
            }
            else
            {
                // If the raycast doesn't hit an anchor, set the second position to a point far away
                lineRenderer.SetPosition(1, LeftshooterTip.position + LeftshooterTip.forward * 1000f);
            }

            // Destroy the hit marker after shooting
            Destroy(hitMarker);
        }
    }


    private void UpdateHitMarkerPosition()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(LeftshooterTip.position, LeftshooterTip.forward, out hitInfo, 1000, webLayers) && hitInfo.collider.CompareTag("Anchor"))
        {
            Vector3 targetPoint = hitInfo.point;
            if (hitMarker == null)
            {
                hitMarker = Instantiate(hitMarkerPrefab, targetPoint, Quaternion.identity);
            }
            else
            {
                hitMarker.transform.position = targetPoint;
            }

            // Print information about the object hit by the raycast
            Debug.Log("Object Hit: " + hitInfo.collider.gameObject.name);

            // Print size of the object hit (assuming it has a collider)
            Collider collider = hitInfo.collider;
            if (collider != null)
            {
                Debug.Log("Size of Object Hit: " + collider.bounds.size);
            }
        }
        else
        {
            // Destroy the hit marker if no valid hit
            Destroy(hitMarker);
        }
    }
}
