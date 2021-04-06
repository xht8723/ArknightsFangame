using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanger : Unit
{
    protected override void OnMouseOver()
    {
        return;
    }

    protected override void OnMouseExit()
    {
        return;
    }

    protected override void move()
    {
        return;
    }

    public override GameObject deploy()
    {
        GameObject EnemyRanger = Instantiate(gameObject);
        return EnemyRanger;
    }

    protected override void Start()
    {
        onUpdateEvent += snapToFloor;
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
    }

    public EnemyRanger(status status)
    {
        Status = status;
        Status.facing = facing.up;
        atkRange = 1;
        moveRange = 2;
    }

}
