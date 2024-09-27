using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReverserWallBehaviour : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f * Time.fixedDeltaTime);
    }
}
