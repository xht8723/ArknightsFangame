using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
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
        GameObject EnemyMelee = Instantiate(gameObject);
        DefaultUnits.setEnemyMelee(EnemyMelee);
        return EnemyMelee;
    }
}
