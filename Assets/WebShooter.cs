using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WebShooter : MonoBehaviour
{
    public InputActionProperty hit;
    public InputActionProperty pull;
    public InputActionProperty fly; // Added fly input action

    [Header("Core Variables")]
    public Transform shooterTip;
    public GameObject SelectedRock = null;
    public Rigidbody player;
    public GameObject webEnd;
    public LineRenderer lineRenderer;
    [Header("Web Settings")]
    public float webStrength = 8.5f;
    public float webDamper = 7f;
    public float webMassScale = 4.5f;
    public float webZipStrength = 5f;
    public float MaxDistance;
    public LayerMask webLayers;
    public GameObject objectToSpawn; // Reference to the GameObject to spawn
    public GameObject hitMarkerPrefab; // Prefab for the hit marker
    public float pullForce = 10f;

    private SpringJoint joint;
    private Vector3 lastHitPoint; // Store the last hit point

    private GameObject hitMarker; // Reference to the instantiated hit marker

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateHitMarkerPosition(); // Update hit marker position continuously
        lineRenderer.SetPosition(0, shooterTip.position);
        Vector3 selectedRockPosition = SelectedRock != null ? SelectedRock.transform.position : shooterTip.position;
        lineRenderer.SetPosition(1, selectedRockPosition);
        if (hit.action.WasPressedThisFrame())
        {
            ShootWeb();
            Destroy(hitMarker); // Destroy the hit marker after shooting
        }

        if (pull.action.WasPressedThisFrame() && SelectedRock != null && SelectedRock.CompareTag("Enemy")) // Added condition to check if SelectedRock is an enemy
        {
            DestroySelectedRockAndChangePosition(shooterTip.position);
        }

        if (fly.action.WasPressedThisFrame() && SelectedRock != null && SelectedRock.CompareTag("Enemy")) // Added condition to check if SelectedRock is an enemy
        {
            LaunchTowardsSelectedRock(selectedRockPosition);
        }
    }


    private void UpdateHitMarkerPosition()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(shooterTip.position, shooterTip.forward, out hitInfo, MaxDistance, webLayers) && hitInfo.collider.CompareTag("Enemy"))
        {
            Vector3 targetPoint = hitInfo.point;
            lastHitPoint = targetPoint; // Store the hit point
            if (hitMarker == null)
            {
                hitMarker = Instantiate(hitMarkerPrefab, targetPoint, Quaternion.identity);
            }
            else
            {
                hitMarker.transform.position = targetPoint;
            }
        }
        else
        {
            Destroy(hitMarker); // Destroy the hit marker if no valid hit
        }
    }

    private void ShootWeb()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(shooterTip.position, shooterTip.forward, out hitInfo, MaxDistance, webLayers))
        {
            Vector3 targetPoint = hitInfo.point;
            lastHitPoint = targetPoint; // Store the hit point
            webEnd.transform.position = targetPoint;

            // Instantiate the specified GameObject at the hit point
            GameObject spawnedObject = Instantiate(objectToSpawn, hitInfo.point, Quaternion.identity);

            joint = hitInfo.collider.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = targetPoint;

            float distanceFromPoint = Vector3.Distance(player.position, targetPoint);
            //The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            // Always update SelectedRock
            SelectedRock = hitInfo.collider.gameObject;
        }
    }

    private void LaunchTowardsSelectedRock(Vector3 newPosition)
    {

        
        StartCoroutine(MoveTowardsDestroyRock(newPosition));
        //if (SelectedRock != null) player.position = SelectedRock.transform.position;
            //player.AddForce(direction * pullForce, ForceMode.Impulse);
        
    }

    private IEnumerator MoveAndDestroyRock(Vector3 targetPosition)
    {
        if (SelectedRock != null)
        {
            // Move the rock towards the target position
            float duration = 0.5f; // Time in seconds to reach the target
            float elapsedTime = 0f;
            Vector3 startingPosition = SelectedRock.transform.position;

            while (elapsedTime < duration)
            {
                SelectedRock.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the rock reaches exactly the target position
            SelectedRock.transform.position = targetPosition;

            // Destroy the rock
            Destroy(SelectedRock);
        }
    }

    private IEnumerator MoveTowardsDestroyRock(Vector3 targetPosition)
    {
        if (SelectedRock != null)
        {
            // Move the rock towards the target position
            float duration = 0.5f; // Time in seconds to reach the target
            float elapsedTime = 0f;
            Vector3 startingPosition = player.transform.position;

            while (elapsedTime < duration)
            {
                player.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the rock reaches exactly the target position
            player.transform.position = targetPosition;

            Destroy(SelectedRock);



        }
    }

    // Call this method from wherever you want to initiate the movement and destruction
    private void DestroySelectedRockAndChangePosition(Vector3 newPosition)
    {
        StartCoroutine(MoveAndDestroyRock(newPosition));
    }
}
