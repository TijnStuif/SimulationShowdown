using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTrapScript : MonoBehaviour
{
    public GameObject trapPrefab; // Prefab of the trap to be placed
    public float cooldownTime = 10f; // Cooldown time in seconds
    private float nextPlaceTime = 0f; // Time when the boss can place the next trap

    void Start()
    {
        // Initialize any necessary components or variables here
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextPlaceTime)
        {
            PlaceTrap();
            nextPlaceTime = Time.time + cooldownTime;
        }
    }

    void PlaceTrap()
    {
        // Define the area to search for valid trap locations
        Vector3 searchCenter = transform.position;
        float searchRadius = 10f; // Adjust as needed

        // Directions to cast rays (forward, backward, left, right)
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        // Find a valid location to place the trap
        foreach (Vector3 direction in directions)
        {
            NavMeshHit hit;
            if (NavMesh.Raycast(searchCenter, searchCenter + direction * searchRadius, out hit, NavMesh.AllAreas))
            {
                Vector3 trapPosition = CalculateTrapPosition(hit.position, direction);
                if (IsLocationValid(trapPosition))
                {
                    Instantiate(trapPrefab, trapPosition, Quaternion.identity);
                    Debug.Log("Trap placed at: " + trapPosition);
                    return;
                }
            }
        }
        Debug.Log("No valid location found to place the trap.");
    }

    Vector3 CalculateTrapPosition(Vector3 hitPosition, Vector3 direction)
    {
        Vector3 trapPosition = hitPosition;

        // Randomly decide where to place the trap
        int placementType = Random.Range(0, 3); // 0: middle, 1: near edge, 2: open area

        if (placementType == 0) // Middle of the hallway
        {
            if (direction == Vector3.left || direction == Vector3.right)
            {
                // Cast rays to the left and right from the hit position
                RaycastHit hitLeft, hitRight;
                bool leftHit = Physics.Raycast(hitPosition, Vector3.left, out hitLeft, Mathf.Infinity);
                bool rightHit = Physics.Raycast(hitPosition, Vector3.right, out hitRight, Mathf.Infinity);

                if (leftHit && rightHit)
                {
                    // Calculate the midpoint between the left and right wall hit points
                    trapPosition = (hitLeft.point + hitRight.point) / 2;
                }
            }
            else if (direction == Vector3.forward || direction == Vector3.back)
            {
                // Cast rays to the forward and backward from the hit position
                RaycastHit hitForward, hitBackward;
                bool forwardHit = Physics.Raycast(hitPosition, Vector3.forward, out hitForward, Mathf.Infinity);
                bool backwardHit = Physics.Raycast(hitPosition, Vector3.back, out hitBackward, Mathf.Infinity);

                if (forwardHit && backwardHit)
                {
                    // Calculate the midpoint between the forward and backward wall hit points
                    trapPosition = (hitForward.point + hitBackward.point) / 2;
                }
            }
        }
        else if (placementType == 1) // Near the edge of the hallway
        {
            if (direction == Vector3.left || direction == Vector3.right)
            {
                // Place near the left or right edge
                RaycastHit hitEdge;
                bool edgeHit = Physics.Raycast(hitPosition, direction, out hitEdge, Mathf.Infinity);
                if (edgeHit)
                {
                    trapPosition = hitEdge.point;
                }
            }
            else if (direction == Vector3.forward || direction == Vector3.back)
            {
                // Place near the forward or backward edge
                RaycastHit hitEdge;
                bool edgeHit = Physics.Raycast(hitPosition, direction, out hitEdge, Mathf.Infinity);
                if (edgeHit)
                {
                    trapPosition = hitEdge.point;
                }
            }
        }
        else if (placementType == 2) // Open area
        {
            // Randomly place within the search radius
            trapPosition = hitPosition + Random.insideUnitSphere * 2f; // Adjust the multiplier as needed
            trapPosition.y = hitPosition.y; // Keep the y-coordinate the same
        }

        return trapPosition;
    }

    bool IsLocationValid(Vector3 position)
    {
        // Check if the location is not occupied by another trap
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Trap"))
            {
                return false;
            }
        }

        // Check if the location is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas))
        {
            return true;
        }

        return false;
    }
}