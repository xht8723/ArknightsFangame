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
        if (isMoving) { return; }
        upArrow.SetActive(true);
        downArrow.SetActive(true);
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    protected override void OnMouseExit()
    {
        if (isMoving) { return; }
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
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

    //Fang's attack deals more dmg when target's hp is lower than half.
    public override void attack(GameObject target)
    {
        if(target.GetComponent<Unit>().Status.hp < target.GetComponent<Unit>().Status.maxHp / 2)
        {
            this.Status.effects.Add(new StatusChangeEffect(this, 1, 150));
        }
        base.attack(target);
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
