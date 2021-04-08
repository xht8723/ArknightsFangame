using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//enums for storing level names.
public enum levels{
    level1 = 1,
    level2 = 2,
    level3 = 3
}

//class used to provide datas to generate a level.
public class LevelInfo
{
    public int level;
    public List<Unit> allies;
    public List<Unit> enemies;
    
    public LevelInfo(levels level, List<Unit> enemies, List<Unit> allies)
    {
        this.level = (int)level;
        this.allies = allies;
        this.enemies = enemies;
    }
}

//data designs for levels.
[System.Serializable]
public static class LevelDesign
{
    static Fang fang = LevelController.levelController.Fang.GetComponent<Fang>();
    static EnemyRanger enemy = LevelController.levelController.EnemyRanger.GetComponent<EnemyRanger>();
    static GameObject[,,] grid = BattleGridsGen.battleGridsGen.gridMatrix;
    static List<Unit> allies = new List<Unit>();
    static List<Unit> enemies = new List<Unit>();

    public static LevelInfo level1
    {
        get
        {
            DefaultUnits.setDefaultFang(fang.gameObject);
            fang.currentPosition = grid[2, 3, 0];
            allies.Add(fang);
            DefaultUnits.setEnemyRanger(enemy.gameObject);
            enemy.currentPosition = grid[3, 3, 0];
            enemies.Add(enemy);
            return new LevelInfo(levels.level1, enemies, allies);
        }

        set
        {

        }
    }
}
