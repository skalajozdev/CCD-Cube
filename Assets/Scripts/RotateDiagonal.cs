using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDiagonal : MonoBehaviour
{
    // The rotation speed of the cube
    public float rotationSpeed = 10.0f;

    void Update()
    {
        // Rotate the cube diagonally around its own axis
        transform.Rotate(Vector3.right + Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
