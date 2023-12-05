using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera m_camera;

    private void Awake()
    {
        m_camera = Camera.main;
    }

    void Update()
    {
        transform.forward = m_camera.transform.forward;
    }
}
