using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fang : Unit
{
    //depolys character onto board.
    public override GameObject deploy()
    {
        GameObject Fang = Instantiate(gameObject);
        DefaultUnits.setDefaultFang(Fang);
        return Fang;
    }

    protected override void OnMouseOver()
    {
        return;
    }

    protected override void OnMouseExit()
    {
        return;
    }

    protected override void OnMouseDown()
    {
        if (!isMoving && (LevelController.levelController.roundStatus.Equals(RoundStatus.move) || LevelController.levelController.roundStatus.Equals(RoundStatus.specialPhase)) && !hasMoved)
        {
            onUpdateEvent -= snapToFloor;
            onUpdateEvent += move;
            isMoving = true;
            MoveEvent();
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        foreach(Skills s in skills)
        {
            if (s.isPassive)
            {
                s.effect();
            }
        }
        base.Start();
    }
}
