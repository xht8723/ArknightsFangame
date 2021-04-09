using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This effect now conflicts with moveAgain skill. a unit must not have this two component at the same time.
public class MoveAgainEffect : Effect
{
    public override void effect()
    {
        unit.Status.effects.Add(this);
        unit.hasMoved = false;
    }

    public override void remove()
    {
        unit.Status.effects.Remove(this);
    }

    public MoveAgainEffect(Unit unit)
    {
        this.unit = unit;
        this.type = EffectType.MoveAgainEffect;
        this.period = 1;
    }

}
