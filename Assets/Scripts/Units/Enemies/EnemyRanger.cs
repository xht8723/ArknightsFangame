using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanger : Enemy
{
    protected override void OnMouseOver()
    {
        return;
    }

    protected override void OnMouseExit()
    {
        return;
    }

    public override GameObject deploy()
    {
        GameObject EnemyRanger = Instantiate(gameObject);
        DefaultUnits.setEnemyRanger(EnemyRanger);
        return EnemyRanger;
    }
}
