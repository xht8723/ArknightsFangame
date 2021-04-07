using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
//buff/debuff types
public enum EffectType
{
    stun,
    fragile,
    statusChange
}
//class for buff/debuff
public abstract class Effect
{

    public EffectType type;
    public int period;

    public abstract void effect();
    public abstract void remove();

}