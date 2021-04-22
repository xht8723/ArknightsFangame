using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//some default unit datas for testing.
public static class DefaultUnits
{
    public static void setDefaultFang(GameObject Fang)
    {
        Fang fang = Fang.GetComponent<Fang>();
        fang.Status = new status(false, 1325, 260, 100, 325, 10);
        fang.Status.facing = facing.down;
        fang.atkRange = 1;
        fang.moveRange = 2;
        fang.skills = new List<Skills>();
        fang.skills.Add(new MoveAgain(fang));
        fang.skills.Add(new AttackUpPassive(fang));
    }

    public static void setEnemyRanger(GameObject ER)
    {
        EnemyRanger er = ER.GetComponent<EnemyRanger>();
        er.isRanged = true;
        er.Status = new status(false, 1400, 100, 0, 240, 5);
        er.Status.facing = facing.up;
        er.atkRange = 10;
        er.moveRange = 2;
        er.visionRange = 10;
        er.skills = new List<Skills>();
    }

    public static void setEnemyMelee(GameObject EM)
    {
        EnemyMelee em = EM.GetComponent<EnemyMelee>();
        em.isRanged = false;
        em.Status = new status(false, 1000, 500, 123, 123, 9);
        em.Status.facing = facing.up;
        em.atkRange = 1;
        em.moveRange = 2;
        em.visionRange = 10;
        em.skills = new List<Skills>();
    }
}
