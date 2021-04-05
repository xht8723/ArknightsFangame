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

public class LevelController : MonoBehaviour
{
    public static LevelController levelController;//make this class an unique object in unity.

    public int level;
    public int roundCounter;
    public RoundStatus roundStatus;

    //prefabs for characters.
    public GameObject Fang;
    public GameObject EnemyRanger;


    public void setupBoard(levels level)
    {

        LevelInfo levelInfo = LevelDesign.level1;
        //Type levelDesign = Type.GetType("LevelDesign");
        //Debug.Log(
        //    levelDesign.GetProperty(level.ToString("G")).GetValue(null));

        //LevelInfo levelInfo = (LevelInfo)levelDesign.GetProperty(level.ToString("G")).GetValue(null);
        //MethodInfo invokeLevel = levelDesign.GetMethod(level.ToString("G"));
        //LevelInfo levelInfo = (LevelInfo)invokeLevel.Invoke(null, null);

        foreach(Unit x in levelInfo.allies)
        {
            Fang.GetComponent<Unit>().deploy(x.currentPosition);
        }

        foreach(Unit x in levelInfo.enemies)
        {
            EnemyRanger.GetComponent<Unit>().deploy(x.currentPosition);
        }

        this.level = (int)level;
        roundCounter = 1;
        roundStatus = RoundStatus.move;
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
