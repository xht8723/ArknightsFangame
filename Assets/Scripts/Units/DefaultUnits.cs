using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultUnits
{
    public static void setDefaultFang(Fang Fang)
    {
        Fang.Status = new status(false, 1325, 260, 100, 325);
        Fang.Status.facing = facing.down;
        Fang.atkRange = 1;
        Fang.moveRange = 2;
    }

    public static void setEnemyRanger(EnemyRanger ER)
    {
        ER.Status = new status(false, 1400, 100, 0, 240);
        ER.Status.facing = facing.up;
        ER.atkRange = 10;
        ER.moveRange = 2;
    }
}
