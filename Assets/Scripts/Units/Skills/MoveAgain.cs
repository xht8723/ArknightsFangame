using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAgain : Skills
{
    public override void effect()
    {
        unit.hasMoved = false;
    }

    public MoveAgain(Unit unit, int cd = 2)
    {
        isPassive = false;
        this.cd = cd;
        this.unit = unit;
    }
}
