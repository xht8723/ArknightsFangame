using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleGridsGen : MonoBehaviour
{

    public static BattleGridsGen battleGridsGen;
    public int col;
    public int row;
    public int gridSizeCoe;
    public GameObject allyFloor;
    public GameObject enemyFloor;
    public int distance;


    public List<GameObject> grids = new List<GameObject>();
    //public List<GameObject> enemyGrids = new List<GameObject>();
    //public List<GameObject> allyGrids = new List<GameObject>();

    public GameObject[,,] gridMatrix;

    public GameObject Fang;

    private void Awake()
    {
        battleGridsGen = this;
    }

    void Start()
    {
        gridGen();
        placeFang();
    }

    public static int[] returnMatrixIndex(GameObject grid)
    {
        int row = BattleGridsGen.battleGridsGen.row;

        string temp = grid.name.Split('d')[1];
        int index = Int32.Parse(temp);
        int[] returns = {index/row, index%row};
        return returns;
    }

    private void placeFang()
    {
        Fang.GetComponent<Fang>().deploy(gridMatrix[2,3,0]);
    }

    private List<GameObject> gridGen() {
        Transform gridHolder = new GameObject("Grids").transform;
        //Transform allyGridHolder = new GameObject("AllyGrids").transform;
        //Transform enemyGridHolder = new GameObject("EnemyGrids").transform;
        int counter = 0;

        for (int i = 0; i < col; i = i + 1) { 
            for(int j = 0; j < row; j = j + 1)
            {
                GameObject newGrid = Instantiate(allyFloor, new Vector3(i*gridSizeCoe, 0f, j*gridSizeCoe), Quaternion.identity, gridHolder);
                newGrid.transform.name = "AllyGrid" + counter.ToString();
                //newGrid.transform.localScale = new Vector3(1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe);
                //allyGrids.Add(newGrid);
                grids.Add(newGrid);
                counter++;
            }
        }
        /*
        counter = 0;

        for (int i = 0; i < col; i = i + 1)
        {
            for (int j = 0; j < row; j = j + 1)
            {
                GameObject newGrid = Instantiate(enemyFloor, new Vector3(i * gridSizeCoe, 0f, j * gridSizeCoe + distance*gridSizeCoe*3), Quaternion.identity, enemyGridHolder);
                //newGrid.transform.localScale = new Vector3(1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe);
                newGrid.transform.name = "EnemyGrid" + counter.ToString();
                enemyGrids.Add(newGrid);
                grids.Add(newGrid);
                counter++;
            }
        }
        */

        //allyGridHolder.SetParent(gridHolder);
        //enemyGridHolder.SetParent(gridHolder);

        gridMatrixGen();

        return grids;
    }

    private GameObject[,,] gridMatrixGen()
    {
        gridMatrix = new GameObject[col, row, 4];

        int counter = 0;

        for (int i = 0; i < col; i++)
        {
            for(int j = 0; j < row; j++)
            {
                gridMatrix[i,j,0] = grids[counter];
                counter++;
            }
        }
        /*
        counter = 0;

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                gridMatrix[i,j,1] = enemyGrids[counter];
                counter++;
            }
        }
        */

        return gridMatrix;
    }

}
