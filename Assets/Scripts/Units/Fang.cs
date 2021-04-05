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
