using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatusChangeEffect : Effect
{
    Unit unit;
    int atkChange;
    int defChange;
    int mrChange;
    int maxHpChange;

    public override void effect()
    {
        unit.Status.atk += atkChange;
        unit.Status.def += defChange;
        unit.Status.mr += mrChange;
        unit.Status.maxHp += maxHpChange;
    }

    public override void remove()
    {
        unit.Status.atk -= atkChange;
        unit.Status.def -= defChange;
        unit.Status.mr -= mrChange;
        unit.Status.maxHp -= maxHpChange;
        unit.Status.effects.Remove(this);
    }

    public StatusChangeEffect(Unit unit, int period = 1, int atkChange = 0, int defChange = 0, int mrChange = 0, int maxHpChange = 0)
    {
        this.unit = unit;
        this.period = period;
        this.atkChange = atkChange;
        this.defChange = defChange;
        this.mrChange = mrChange;
        this.maxHpChange = maxHpChange;
    }

}
