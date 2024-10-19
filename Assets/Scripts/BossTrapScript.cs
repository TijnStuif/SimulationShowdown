using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTrapScript : MonoBehaviour
{
    public GameObject trapPrefab; 
    public float cooldownTime = 10f; 
    private float nextPlaceTime = 0f; 

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
        Vector3 searchCenter = transform.position;
        float searchRadius = 100f; 
        
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

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

        int placementType = Random.Range(0, 3); // 0: middle, 1: near edge, 2: open area

        if (placementType == 0) 
        {
            if (direction == Vector3.left || direction == Vector3.right)
            {
                RaycastHit hitLeft, hitRight;
                bool leftHit = Physics.Raycast(hitPosition, Vector3.left, out hitLeft, Mathf.Infinity);
                bool rightHit = Physics.Raycast(hitPosition, Vector3.right, out hitRight, Mathf.Infinity);

                if (leftHit && rightHit)
                {
                    trapPosition = (hitLeft.point + hitRight.point) / 2;
                }
            }
            else if (direction == Vector3.forward || direction == Vector3.back)
            {
                RaycastHit hitForward, hitBackward;
                bool forwardHit = Physics.Raycast(hitPosition, Vector3.forward, out hitForward, Mathf.Infinity);
                bool backwardHit = Physics.Raycast(hitPosition, Vector3.back, out hitBackward, Mathf.Infinity);

                if (forwardHit && backwardHit)
                {
                    trapPosition = (hitForward.point + hitBackward.point) / 2;
                }
            }
        }
        else if (placementType == 1) 
        {
            if (direction == Vector3.left || direction == Vector3.right)
            {
                RaycastHit hitEdge;
                bool edgeHit = Physics.Raycast(hitPosition, direction, out hitEdge, Mathf.Infinity);
                if (edgeHit)
                {
                    trapPosition = hitEdge.point;
                }
            }
            else if (direction == Vector3.forward || direction == Vector3.back)
            {
                RaycastHit hitEdge;
                bool edgeHit = Physics.Raycast(hitPosition, direction, out hitEdge, Mathf.Infinity);
                if (edgeHit)
                {
                    trapPosition = hitEdge.point;
                }
            }
        }
        else if (placementType == 2) 
        {
            trapPosition = hitPosition + Random.insideUnitSphere * 2f; 
            trapPosition.y = hitPosition.y; 
        }

        return trapPosition;
    }

    bool IsLocationValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Trap"))
            {
                return false;
            }
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas))
        {
            return true;
        }

        return false;
    }
}