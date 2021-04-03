using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelController : MonoBehaviour
{
    public static LevelController levelController;

    private Ray ray;
    private RaycastHit hit;

    public Transform castRay(int layerMask = 1 << 0)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.transform;
        }
        return null;
    }


    private void Awake()
    {
        levelController = this;
    }

    void Update()
    {
        //castRay();
    }
}
