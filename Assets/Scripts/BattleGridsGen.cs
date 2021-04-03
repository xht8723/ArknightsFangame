using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridsGen : MonoBehaviour
{
    public int col;
    public int row;
    public int enemyFieldNum;
    public int gridSizeCoe;
    public GameObject allyFloor;
    public GameObject enemyFloor;
    public int distance;


    public List<GameObject> grids = new List<GameObject>();
    public List<GameObject> enemyGrids = new List<GameObject>();
    public List<GameObject> allyGrids = new List<GameObject>();

    public GameObject Fang;


    void Start()
    {
        gridGen();
        placeFang();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void placeFang()
    {
        Fang.GetComponent<Fang>().deploy(allyGrids[7]);
    }

    private List<GameObject> gridGen() {
        Transform gridHolder = new GameObject("Grids").transform;
        Transform allyGridHolder = new GameObject("AllyGrids").transform;
        Transform enemyGridHolder = new GameObject("EnemyGrids").transform;
        int counter = 0;

        for (int i = 0; i < col; i = i + 1) { 
            for(int j = 0; j < row; j = j + 1)
            {
                GameObject newGrid = Instantiate(allyFloor, new Vector3(i*gridSizeCoe, 0f, j*gridSizeCoe), Quaternion.identity, allyGridHolder);
                newGrid.transform.name = "AllyGrid" + counter.ToString();
                //newGrid.transform.localScale = new Vector3(1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe, 1.0f / (float)gridSizeCoe);
                allyGrids.Add(newGrid);
                grids.Add(newGrid);
                counter++;
            }
        }

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

        allyGridHolder.SetParent(gridHolder);
        enemyGridHolder.SetParent(gridHolder);

        return grids;
    }
}
