using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public bool BillboardX = true;
    public bool BillboardY = true;
    public bool BillboardZ = true;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);

        if(!BillboardX || !BillboardY || !BillboardZ) {
            transform.rotation = Quaternion.Euler(
                BillboardX ? transform.rotation.eulerAngles.x : 0f, 
                BillboardY ? transform.rotation.eulerAngles.y : 0f, 
                BillboardZ ? transform.rotation.eulerAngles.z : 0f);
        }
    }
}
