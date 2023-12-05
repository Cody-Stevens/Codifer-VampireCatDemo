using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public Vector3 RotateValues;

    void Update()
    {
        transform.Rotate(RotateValues * Time.deltaTime);
    }
}
