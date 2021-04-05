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

    public override GameObject deploy(GameObject position)
    {
        GameObject EnemyRanger = Instantiate(gameObject);
        EnemyRanger ER = EnemyRanger.GetComponent<EnemyRanger>();
        ER.currentPosition = position;
        return EnemyRanger;
    }

    public override GameObject deploy()
    {
        GameObject EnemyRanger = Instantiate(gameObject);
        EnemyRanger ER = EnemyRanger.GetComponent<EnemyRanger>();
        return EnemyRanger;
    }

    public override void attack(GameObject target)
    {
        attackEvent(target);
        target.GetComponent<Unit>().receiveDmg(gameObject);
    }

    protected override void Start()
    {
        onUpdateEvent += snapToFloor;
        onMoveEvent += resrictionVisual;
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
        Status = new status(false, 1400, 100, 0, 240);
        Status.facing = facing.right;
        atkRange = 10;
        moveRange = 2;
    }

    public EnemyRanger(GameObject startPosition)
    {
        Status = new status(false, 1400, 100, 0, 240);
        Status.facing = facing.right;
        atkRange = 1;
        moveRange = 2;
        currentPosition = startPosition;
    }

}
