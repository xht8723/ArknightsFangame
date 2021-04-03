using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public status Status;
    public event Action<Unit, Unit> onAttackEvent;
    public event Action<Unit, Unit> onReceiveDmgEvent;
    protected event Action onUpdateEvent;
    protected event Action onMoveEvent;
    public int atkRange;
    public int moveRange;
    public GameObject currentPosition;
    public GameObject lastPosition;


    bool isMoving = false;


    public void UpdateEvent()
    {
        if (onUpdateEvent != null)
        {
            onUpdateEvent();
        }
    }

    public void attackEvent(GameObject target)
    {
        if(onAttackEvent != null)
        {
            onAttackEvent(this, target.GetComponent<Unit>());
        }
    }

    public void ReceiveDmgEvent(GameObject opponent)
    {
        if(onReceiveDmgEvent != null)
        {
            onReceiveDmgEvent(this, opponent.GetComponent<Unit>());
        }
    }

    public void MoveEvent()
    {
        if(onMoveEvent != null)
        {
            onMoveEvent();
        }
    }

    public abstract GameObject deploy(GameObject position);

    protected abstract void OnMouseOver();

    protected abstract void OnMouseExit();

    protected virtual void OnMouseDown()
    {
        if (!isMoving)
        {
            onUpdateEvent -= snapToFloor;
            onUpdateEvent += move;
            onMoveEvent += moveRestrcition;
            isMoving = true;
        }
    }

    private Ray ray;
    private RaycastHit hit;
    protected virtual void move()
    {
        MoveEvent();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("floor")))
        {
            transform.position = hit.transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                lastPosition = currentPosition;
                currentPosition = hit.transform.gameObject;
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                onMoveEvent -= moveRestrcition;
                isMoving = false;
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                onMoveEvent -= moveRestrcition;
                isMoving = false;
                return;
            }
        }
    }

    protected virtual void moveRestrcition()
    {

    }

    protected virtual List<GameObject> restriction()
    {
        GameObject[,,] matrix = BattleGridsGen.battleGridsGen.gridMatrix;
        foreach(GameObject x in matrix)
        {

        }

        return null;
    }

    protected virtual List<GameObject> traceViableGrids(GameObject[,,] matrix)
    {
        List<GameObject> viable = new List<GameObject>();
        int[] currentIndex = BattleGridsGen.returnMatrixIndex(currentPosition);

        for (int i = 0; i < BattleGridsGen.battleGridsGen.col; i++)
        {
            for(int j = 0; j < BattleGridsGen.battleGridsGen.row; j++)
            {
                int[] index = BattleGridsGen.returnMatrixIndex(matrix[i, j, 0]);
                if(Math.Abs((index[0] - currentIndex[0])) + Math.Abs((index[1] - currentIndex[1])) <= moveRange)
                {
                    viable.Add(matrix[i, j, 0]);
                }

            }
        }
        return viable;
    }

    public abstract void attack(GameObject target);

    public virtual void receiveDmg(GameObject opponent)
    {
        ReceiveDmgEvent(opponent);
        status opponentStatus = opponent.GetComponent<Unit>().Status;
        int dmg = opponentStatus.atk;

        if (Status.isBack(opponent)) { dmg = dmg * 2; }

        if (opponentStatus.isMagic)
        {
            Status.hp = Status.hp - (dmg * Status.mr);
        }
        else
        {
            if(Status.def >= dmg)
            {
                Status.hp -= 1;
            }
            else
            {
                Status.hp = Status.hp - (dmg - Status.def);
            }
        }

        if (isDead()) { Destroy(this.gameObject); }
    }

    protected virtual void snapToFloor()
    {
        transform.position = currentPosition.transform.position;
    }

    protected virtual Boolean isDead()
    {
        if(Status.hp <= 0)
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        onUpdateEvent += snapToFloor;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateEvent();
    }
}
