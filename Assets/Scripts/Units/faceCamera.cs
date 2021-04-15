using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceCamera : MonoBehaviour
{
    Camera maincamera;

    private void Awake()
    {
        maincamera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(maincamera.transform.position);
    }
}
