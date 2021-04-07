using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpPassive : Skills
{

    public override void effect()
    {
        StatusChangeEffect e = new StatusChangeEffect(unit, 1, 100);
        unit.Status.effects.Add(e);
        e.effect();
    }

    public AttackUpPassive(Unit unit)
    {
        isPassive = true;
        cd = 0;
        this.unit = unit;
    }
}
