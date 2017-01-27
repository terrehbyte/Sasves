using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 axisOfRotation;
    public Space rotationSpace = Space.World;
    public float degreesPerSecond;

    void Update()
    {
        transform.Rotate(axisOfRotation, degreesPerSecond * Time.deltaTime, rotationSpace);
    }
}
