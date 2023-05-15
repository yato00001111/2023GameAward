using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCube : MonoBehaviour
{
    float x = -8;
    void FixedUpdate()
    {
        Vector3 p = new Vector3(x, 4, 0);
        x += 0.1f;
        transform.position = p;
    }
}
