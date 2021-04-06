using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fang : Unit
{
    public override GameObject deploy()
    {
        GameObject Fang = Instantiate(gameObject);
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

    // Start is called before the first frame update
    protected override void Start()
    {
        onUpdateEvent += snapToFloor;
        onMoveEvent += resrictionVisual;
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
    }


    public Fang(status status)
    {
        Status = status;
        Status.facing = facing.down;
        atkRange = 1;
        moveRange = 2;
    }
}
