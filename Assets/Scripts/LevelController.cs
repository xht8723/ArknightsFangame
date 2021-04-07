using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

//enum for different phase of a round.
public enum RoundStatus{ 
    specialPhase,
    move,
    attack
}

//contols everything within a level, including phase change, deploy characters, storing round information .etc
public class LevelController : MonoBehaviour
{
    public static LevelController levelController;//make this class an unique object in unity.

    //prefabs for characters.
    public GameObject Fang;
    public GameObject EnemyRanger;

    public int level;
    public int roundCounter;
    public RoundStatus roundStatus;
    public List<Unit> aliveUnits = new List<Unit>();//list for alive units on boards.
    public List<attack> viableAttacks = new List<attack>();//list for all viable attacks after move phase.

    public event Action onMoveEndEvent;
    public event Action onMoveStartEvent;
    public event Action onAttackEndEvent;
    public event Action onAttackStartEvent;
    public event Action onSpecialPhaseEndEvent;
    public event Action onSpecialPhaseStartEvent;

    public void MoveEndEvent()
    {
        if (onMoveEndEvent != null)
        {
            onMoveEndEvent();
        }
    }

    public void MoveStartEvent()
    {
        if (onMoveStartEvent != null)
        {
            onMoveStartEvent();
        }
    }

    public void AttackEndEvent()
    {
        if (onAttackEndEvent != null)
        {
            onAttackEndEvent();
        }
    }


    public void AttackStartEvent()
    {
        if (onAttackStartEvent != null)
        {
            onAttackStartEvent();
        }
    }


    public void SpecialPhaseEndEvent()
    {
        if (onSpecialPhaseEndEvent != null)
        {
            onSpecialPhaseEndEvent();
        }
    }


    public void SpecialPhaseStartEvent()
    {
        if (onSpecialPhaseStartEvent != null)
        {
            onSpecialPhaseStartEvent();
        }
    }

    //load level data from LevelDesign class and place all units.
    public void setupBoard(levels level)
    {
        Type levelDesign = Type.GetType("LevelDesign");
        LevelInfo levelInfo = (LevelInfo)levelDesign.GetProperty(level.ToString("G")).GetValue(null);

        foreach(Unit x in levelInfo.allies)
        {
            GameObject newAlly = x.deploy();
            aliveUnits.Add(newAlly.GetComponent<Unit>());
        }

        foreach(Unit x in levelInfo.enemies)
        {
            GameObject newEnemy = x.deploy();
            aliveUnits.Add(newEnemy.GetComponent<Unit>());
        }

        this.level = (int)level;
        roundCounter = 1;
        roundStatus = RoundStatus.move;
    }

    //switch phases
    public void goNextPhase()
    {
        switch (roundStatus)
        {
            case RoundStatus.move:
                MoveEndEvent();
                roundStatus = RoundStatus.attack;
                updateAttacks();
                AttackStartEvent();
                return;

            case RoundStatus.attack:
                AttackEndEvent();
                roundStatus = RoundStatus.specialPhase;
                SpecialPhaseStartEvent();
                return;

            case RoundStatus.specialPhase:
                SpecialPhaseEndEvent();
                roundCounter++;
                roundStatus = RoundStatus.move;
                foreach(Unit u in aliveUnits)
                {
                    u.resetFlags();
                }
                MoveStartEvent();
                return;
        }
    }

    //healper method to check if a unit is on a grid.
    public bool detectIfUnitOnGrid(Unit unit, GameObject grid)
    {
        if(unit.currentPosition == grid)
        {
            return true;
        }
        return false;
    }

    //store a unit's attackable grids within a unit's attack range.
    public List<GameObject> inRangeGrids(Unit unit)
    {
        int[] position = BattleGridsGen.returnMatrixIndex(unit.currentPosition);
        GameObject[,,] grids = BattleGridsGen.battleGridsGen.gridMatrix;
        List<GameObject> inRangeGrids = new List<GameObject>();
        string facing = unit.Status.facing.ToString();
        switch (facing)
        {
            case "up":
                try
                {
                    for (int i = 1; i <= unit.atkRange; i++)
                    {
                        inRangeGrids.Add(grids[position[0] - i, position[1], 0]);
                        grids[position[0] - i, position[1], 0].gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    break;
                }
                catch (Exception)
                {
                    break;
                }

            case "down":
                try
                {
                    for (int i = 1; i <= unit.atkRange; i++)
                    {
                        inRangeGrids.Add(grids[position[0] + i, position[1], 0]);
                    }
                    Debug.Log("in down");
                    break;
                }
                catch (Exception)
                {
                    Debug.Log("Index out of range");
                    break;
                }

            case "left":
                try
                {
                    for (int i = 1; i <= unit.atkRange; i++)
                    {
                        inRangeGrids.Add(grids[position[0], position[1] - i, 0]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
                break;

            case "right":
                try
                {
                    for (int i = 1; i <= unit.atkRange; i++)
                    {
                        inRangeGrids.Add(grids[position[0], position[1] + i, 0]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
                break;
        }

        return inRangeGrids;
    }

    //stores viable attacks if there are attackable units in their attack range.
    public attack detectViableAttacks(List<GameObject> inRangeGrids, Unit unit)
    {
        foreach (GameObject x in inRangeGrids)
        {
            foreach (Unit u in aliveUnits)
            {
                switch (unit.transform.tag)
                {
                    case "enemy":
                        if (u.transform.tag == "ally" && detectIfUnitOnGrid(u, x))
                        {
                            return new attack(unit, u);
                        }
                        else
                        {
                            continue;
                        }

                    case "ally":
                        if (u.transform.tag == "enemy" && detectIfUnitOnGrid(u, x))
                        {
                            return new attack(unit, u);
                        }
                        else
                        {
                            continue;
                        }
                }
            }
        }

        Debug.Log("No valid attacks");
        return new attack(true);
    }

    //updates viable attackes.
    public void updateAttacks()
    {
        viableAttacks.Clear();
        foreach(Unit u in aliveUnits)
        {
            viableAttacks.Add(detectViableAttacks(inRangeGrids(u), u));  
        }
    }

    //attacks.
    public void excecuteAttacks()
    {
        if (viableAttacks.Contains(new attack(true)) || viableAttacks.Count == 0)
        {
            Debug.Log("no viable attacks");
            return;
        }

        foreach(attack a in viableAttacks)
        {
            a.self.attack(a.target.gameObject);
        }

        viableAttacks.Clear();
    }

    //simulate attacks for preview the combat result before acually attacks. used for preview UI.
    public void simulateAttackResult(Unit self, Unit target, out status selfResult, out status targetResult)
    {
        status selfData = new status(self.Status);
        status targetData = new status(target.Status);
        int dmg = selfData.atk;

        if (targetData.isBack(self.gameObject)) { dmg = dmg * 2; }

        if (selfData.isMagic)
        {
            targetData.hp = targetData.hp - (dmg * targetData.mr);
        }
        else
        {
            if (targetData.def >= dmg)
            {
                targetData.hp -= 1;
            }
            else
            {
                targetData.hp = targetData.hp - (dmg - targetData.def);
            }
        }

        selfResult = selfData;
        targetResult = targetData;
    }


    private void Awake()
    {
        levelController = this;
    }

    private void Start()
    {
        setupBoard(levels.level1);
    }

    void Update()
    {

    }
}

//helper struct that stores an attack information.
public struct attack
{
    public Unit self;
    public Unit target;
    public bool isEmpty;

    public attack(Unit self, Unit target)
    {
        this.self = self;
        this.target = target;
        isEmpty = false;
    }

    public attack(bool isEmpty)
    {
        this.self = null;
        this.target = null;
        this.isEmpty = isEmpty;
    }
}

