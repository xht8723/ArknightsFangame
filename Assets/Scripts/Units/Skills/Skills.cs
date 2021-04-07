using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Skills
{
    public bool isPassive;
    public Unit unit;
    public int cd;

    public abstract void effect();


}
