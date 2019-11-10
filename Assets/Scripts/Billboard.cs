using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;
    private Camera Camera
    {
        get
        {
            if (!_camera)
                _camera = Camera.main;

            return _camera;
        }
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.transform.rotation * Vector3.forward,
            Camera.transform.rotation * Vector3.up);
    }
}
