using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//a class used to generated floor and make it into a matrix system.
public class BattleGridsGen : MonoBehaviour
{
    public static BattleGridsGen battleGridsGen;//make this class an unique object in unity
    public int col;
    public int row;
    public int gridSizeCoe;
    public GameObject allyFloor;//prefab for floor
    public GameObject enemyFloor;
    public int distance;//no use for now


    public List<GameObject> grids = new List<GameObject>();

    public GameObject[,,] gridMatrix;//grid matrix
    public Transform gridHolder;

    private void Awake()
    {
        battleGridsGen = this;
    }

    void Start()
    {
        gridGen();
    }

    //return the grid object's index number in matrix.
    public static int[] returnMatrixIndex(GameObject grid)
    {
        int row = BattleGridsGen.battleGridsGen.row;

        string temp = grid.name.Split('d')[1];
        int index = Int32.Parse(temp);
        int[] returns = {index/row, index%row};
        return returns;
    }

    //Generates floor.
    private List<GameObject> gridGen() {
        gridHolder = new GameObject("Grids").transform;
        int counter = 0;

        for (int i = 0; i < col; i = i + 1) { 
            for(int j = 0; j < row; j = j + 1)
            {
                GameObject newGrid = Instantiate(allyFloor, new Vector3(i*gridSizeCoe, 0f, j*gridSizeCoe), Quaternion.identity, gridHolder);
                newGrid.transform.name = "AllyGrid" + counter.ToString();
                grids.Add(newGrid);
                counter++;
            }
        }
        gridMatrixGen();

        return grids;
    }

    //Generate grid matrix.
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
        return gridMatrix;
    }

}
