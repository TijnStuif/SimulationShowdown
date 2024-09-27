using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReverserWallBehaviour : MonoBehaviour
{
    public GameObject inputReverserWall;

    void FixedUpdate()
    {
        if (!inputReverserWall.activeSelf) return;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f * Time.fixedDeltaTime);
    }
}
