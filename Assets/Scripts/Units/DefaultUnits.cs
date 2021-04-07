using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//some default unit datas for testing.
public static class DefaultUnits
{
    public static void setDefaultFang(GameObject Fang)
    {
        Fang fang = Fang.GetComponent<Fang>();
        fang.Status = new status(false, 1325, 260, 100, 325);
        fang.Status.facing = facing.down;
        fang.atkRange = 1;
        fang.moveRange = 2;
        fang.skills = new List<Skills>();
        fang.skills.Add(new MoveAgain(fang));
        fang.skills.Add(new AttackUpPassive(fang));
    }

    public static void setEnemyRanger(EnemyRanger ER)
    {
        ER.Status = new status(false, 1400, 100, 0, 240);
        ER.Status.facing = facing.up;
        ER.atkRange = 10;
        ER.moveRange = 2;
    }
}
