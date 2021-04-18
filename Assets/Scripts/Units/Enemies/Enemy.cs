using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : Unit
{
    public bool isRanged;
    public int visionRange;
    protected List<GameObject> vision;

    //return the girds that are in vision range.
    protected virtual List<GameObject> calculateVision(GameObject[,,] matrix)
    {
        List<GameObject> vision = new List<GameObject>();
        int[] currentIndex = BattleGridsGen.returnMatrixIndex(currentPosition);

        for (int i = 0; i < BattleGridsGen.battleGridsGen.col; i++)
        {
            for (int j = 0; j < BattleGridsGen.battleGridsGen.row; j++)
            {
                if ((Math.Abs((i - currentIndex[0])) + Math.Abs((j - currentIndex[1]))) <= visionRange)
                {
                    vision.Add(matrix[i, j, 0]);
                }
            }
        }
        this.vision = vision;
        return vision;
    }

    //returns the list of allies that are in the vision range.
    protected virtual List<Unit> traceAlly()
    {
        List<Unit> allies = new List<Unit>();
        foreach(GameObject g in vision)
        {
            foreach(Unit u in LevelController.levelController.aliveUnits)
            {
                if (u.transform.tag == "ally" && LevelController.levelController.detectIfUnitOnGrid(u, g))
                {
                    allies.Add(u);
                }
            }
        }
        
        return allies;
    }

    protected override void OnMouseDown()
    {
        return;
    }

    protected override void OnMouseUp()
    {
        return;
    }

    //move logic.
    protected override void move()
    {
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
        calculateVision(BattleGridsGen.battleGridsGen.gridMatrix);
        List<Unit> hasAlly = traceAlly();
        Unit closestAlly = null;
        int distance = BattleGridsGen.battleGridsGen.col * BattleGridsGen.battleGridsGen.row;
        foreach(Unit u in hasAlly)
        {
            int temp = calculateDistance(this, u);
            if(temp < distance)
            {
                closestAlly = u;
                distance = temp;
            }
            else
            {
                continue;
            }
        }

        if (isRanged)
        {
            moveAway(closestAlly);
            MoveEvent();
            isTurn = false;
        }
        else
        {
            moveToward(closestAlly);
            MoveEvent();
            isTurn = false;
        }

        LevelController.levelController.startMove();
    }

    protected void canMove()
    {
        if (this.isTurn)
        {
            move();
        }
    }

    protected override void Start()
    {
        foreach (Skills s in skills)
        {
            if (s.isPassive)
            {
                s.effect();
            }
        }
        onUpdateEvent += canMove;
        base.Start();
    }
}
