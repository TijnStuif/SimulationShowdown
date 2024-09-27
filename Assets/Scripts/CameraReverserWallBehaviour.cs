using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReverserWallBehaviour : MonoBehaviour
{
    public GameObject cameraReverserWall;

    void FixedUpdate()
    {
        if (!cameraReverserWall.activeSelf) return;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f * Time.fixedDeltaTime);
    }
}
