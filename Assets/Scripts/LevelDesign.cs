using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum levels{
    level1 = 1,
}

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


[System.Serializable]
public static class LevelDesign
{
    static GameObject[,,] grid = BattleGridsGen.battleGridsGen.gridMatrix; 
    public static LevelInfo level1
    {
        get
        {
            List<Unit> allies = new List<Unit>();
            Fang fang = LevelController.levelController.Fang.GetComponent<Fang>();
            fang.currentPosition = grid[2, 3, 0];
            allies.Add(fang);
            List<Unit> enemies = new List<Unit>();
            Unit enemy = LevelController.levelController.EnemyRanger.GetComponent<Unit>();
            enemy.currentPosition = grid[5, 6, 0];
            enemies.Add(enemy);
            return new LevelInfo(levels.level1, enemies, allies);
        }

        set
        {

        }
    }
}
