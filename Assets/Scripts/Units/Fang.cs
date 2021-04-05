using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fang : Unit
{
    public override GameObject deploy(GameObject position)
    {
        GameObject Fang = Instantiate(gameObject);
        Fang datas = Fang.GetComponent<Fang>();
        datas.currentPosition = position;
        return Fang;
    }
    public override GameObject deploy()
    {
        GameObject Fang = Instantiate(gameObject);
        Fang datas = Fang.GetComponent<Fang>();
        return Fang;
    }

    protected override void OnMouseOver()
    {
        return;
    }

    protected override void OnMouseExit()
    {
        return;
    }


    
    /*
    int layerMask = 1 << 3;
    private Renderer changedRenderer;
    private Material originalMat;
    protected override void move()
    {

        layerMask = ~layerMask;
        Transform hit = LevelController.levelController.castRay(layerMask);
        if (hit != null)
        {
            if(changedRenderer == null)
            {
                originalMat = hit.GetComponent<Renderer>().material;
                hit.GetComponent<Renderer>().material.color = Color.black;
                changedRenderer = hit.GetComponent<Renderer>();
            }
            else
            {
                if(changedRenderer != hit.GetComponent<Renderer>())
                {
                    changedRenderer.material = originalMat;
                }
            }
        }
    }
    */


    /*
    private Ray ray;
    private RaycastHit hit;

    
    protected override void move()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("floor")))
        {
            Debug.Log(hit.transform.name);
            transform.position = hit.transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                lastPosition = currentPosition;
                currentPosition = hit.transform.gameObject;
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                isMoving = false;
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                isMoving = false;
                return;
            }
        }
    }
    */

    public override void attack(GameObject target)
    {
        attackEvent(target);
        target.GetComponent<Unit>().receiveDmg(gameObject);
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        onUpdateEvent += snapToFloor;
        onMoveEvent += resrictionVisual;
        Status = new status(false, 1325, 260, 0, 325);
        Status.facing = facing.right;
        atkRange = 1;
        moveRange = 2;
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
    }


    public Fang(GameObject startPosition)
    {
        Status = new status(false, 1325, 260, 0, 325);
        Status.facing = facing.right;
        atkRange = 1;
        moveRange = 2;
        currentPosition = startPosition;
    }
}
