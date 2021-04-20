using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UnityEngine.UI;

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
    [Header("UI references")]
    public GameObject UI;
    public GameObject phasePlate_move;
    public GameObject phasePlate_attack;
    public GameObject phasePlate_special;

    [Header("Unit prefabs")]
    //prefabs for characters.
    public GameObject Fang;
    public GameObject EnemyRanger;

    public int level;
    public int roundCounter;
    public RoundStatus roundStatus;
    public List<Unit> aliveUnits = new List<Unit>();//list for alive units on boards.
    public List<attack> viableAttacks = new List<attack>();//list for all viable attacks after move phase.
    public SortedList<int, Unit> speedList = new SortedList<int, Unit>();

    public event Action onMoveEndEvent;
    public event Action onMoveStartEvent;
    public event Action onAttackEndEvent;
    public event Action onAttackStartEvent;
    public event Action onSpecialPhaseEndEvent;
    public event Action onSpecialPhaseStartEvent;
    public event Action onUpdateEvent;

    public void UpdateEvent()
    {
        if(onUpdateEvent != null)
        {
            onUpdateEvent();
        }
    }
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

    public void caculateSpeed()
    {
        speedList.Clear();
        foreach(Unit x in aliveUnits)
        {
            speedList.Add(x.Status.speed, x);
        }
        startMove();
    }

    public void startMove()
    {
        if (!roundStatus.Equals(RoundStatus.move))
        {
            return;
        }
        if (speedList.Count == 0)
        {
            goNextPhase();
            return;
        }
        Unit unit = speedList.Values[speedList.Count - 1];
        speedList.RemoveAt(speedList.Count - 1);
        unit.isTurn = true;
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
        onUpdateEvent += playPhaseChangeAnimation;
        caculateSpeed();
    }

    float time = 0;
    //switch phases
    public void goNextPhase()
    {
        switch (roundStatus)
        {
            case RoundStatus.move:
                MoveEndEvent();
                roundStatus = RoundStatus.attack;
                time = 0;
                Time.timeScale = 0;
                onUpdateEvent += playPhaseChangeAnimation;
                AttackStartEvent();
                return;

            case RoundStatus.attack:
                AttackEndEvent();
                roundStatus = RoundStatus.specialPhase;
                time = 0;
                Time.timeScale = 0;
                onUpdateEvent += playPhaseChangeAnimation;
                SpecialPhaseStartEvent();
                return;

            case RoundStatus.specialPhase:
                SpecialPhaseEndEvent();
                roundCounter++;
                roundStatus = RoundStatus.move;
                time = 0;
                Time.timeScale = 0;
                onUpdateEvent += playPhaseChangeAnimation;
                foreach (Unit u in aliveUnits)
                {
                    u.resetFlags();
                }
                MoveStartEvent();
                return;
        }
    }

    public void playPhaseChangeAnimation()
    {
        switch (roundStatus)
        {
            case RoundStatus.move:
                if(phasePlate_move.GetComponent<Image>().fillAmount >= 1f)
                {
                    if(time <= 1f)
                    {
                        time += Time.unscaledDeltaTime;
                    }
                    else
                    {
                        onUpdateEvent -= playPhaseChangeAnimation;
                        onUpdateEvent += stopPhaseChangeAnimation;
                    }
                    return;
                }
                phasePlate_move.SetActive(true);
                phasePlate_move.GetComponent<Image>().fillOrigin = 0;
                phasePlate_move.GetComponent<Image>().fillAmount += Time.unscaledDeltaTime / 0.5f;
                break;
            case RoundStatus.attack:
                if (phasePlate_attack.GetComponent<Image>().fillAmount >= 1f)
                {
                    if (time <= 1f)
                    {
                        time += Time.unscaledDeltaTime;
                    }
                    else
                    {
                        onUpdateEvent -= playPhaseChangeAnimation;
                        onUpdateEvent += stopPhaseChangeAnimation;
                    }
                    return;
                }
                phasePlate_attack.SetActive(true);
                phasePlate_attack.GetComponent<Image>().fillOrigin = 0;
                phasePlate_attack.GetComponent<Image>().fillAmount += Time.unscaledDeltaTime / 0.5f;
                break;

            case RoundStatus.specialPhase:
                if (phasePlate_special.GetComponent<Image>().fillAmount >= 1f)
                {
                    if (time <= 1f)
                    {
                        time += Time.unscaledDeltaTime;
                    }
                    else
                    {
                        onUpdateEvent -= playPhaseChangeAnimation;
                        onUpdateEvent += stopPhaseChangeAnimation;
                    }
                    return;
                }
                phasePlate_special.SetActive(true);
                phasePlate_special.GetComponent<Image>().fillOrigin = 0;
                phasePlate_special.GetComponent<Image>().fillAmount += Time.unscaledDeltaTime / 0.5f;
                break;
        }
    }

    public void stopPhaseChangeAnimation()
    {
        switch (roundStatus)
        {
            case RoundStatus.move:
                if (phasePlate_move.GetComponent<Image>().fillAmount <= 0f)
                {
                    phasePlate_move.SetActive(false);
                    Time.timeScale = 1;
                    onUpdateEvent -= stopPhaseChangeAnimation;
                    return;
                }
                phasePlate_move.GetComponent<Image>().fillOrigin = 1;
                phasePlate_move.GetComponent<Image>().fillAmount -= Time.unscaledDeltaTime / 0.5f;
                break;

            case RoundStatus.attack:
                if (phasePlate_attack.GetComponent<Image>().fillAmount <= 0f)
                {
                    phasePlate_attack.SetActive(false);
                    Time.timeScale = 1;
                    onUpdateEvent -= stopPhaseChangeAnimation;
                    return;
                }
                phasePlate_attack.GetComponent<Image>().fillOrigin = 1;
                phasePlate_attack.GetComponent<Image>().fillAmount -= Time.unscaledDeltaTime / 0.5f;
                break;

            case RoundStatus.specialPhase:
                if (phasePlate_special.GetComponent<Image>().fillAmount <= 0f)
                {
                    phasePlate_special.SetActive(false);
                    Time.timeScale = 1;
                    onUpdateEvent -= stopPhaseChangeAnimation;
                    return;
                }
                phasePlate_special.GetComponent<Image>().fillOrigin = 1;
                phasePlate_special.GetComponent<Image>().fillAmount -= Time.unscaledDeltaTime / 0.5f;
                break;
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

    public void updatesFOW()
    {

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
        onAttackStartEvent += updateAttacks;
        onMoveStartEvent += caculateSpeed;
    }

    void Update()
    {
        UpdateEvent();
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

