using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;


//abstract class for all characters.
public abstract class Unit : MonoBehaviour
{
    public status Status;
    public event Action<Unit, Unit> onAttackEvent;
    public event Action<Unit, Unit> onReceiveDmgEvent;
    public event Action onKillEvent;
    public event Action onUpdateEvent;
    public event Action onMoveEvent;
    public int atkRange;
    public int moveRange;
    public List<Skills> skills;
    public int cd;
    public bool hasMoved = false;
    public bool hasAttacked = false;
    public bool hasSpecial = false;
    public GameObject currentPosition;
    public GameObject lastPosition;
    public bool isMoving = false;
    public bool isTurn = false;
    public List<GameObject> viableRoutes;//stores vaible grids that this unit can move to.

    //some referecens for unity editor. do not change.
    [Header("UIs")]
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;
    Camera maincamera;
    public GameObject sprite;
    public GameObject indicatorFloor;

    public void killEvent()
    {
        if(onKillEvent != null)
        {
            onKillEvent();
        }
    }

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

    public void MoveEvent()
    {
        if (onMoveEvent != null)
        {
            onMoveEvent();
        }
    }

    public void ReceiveDmgEvent(GameObject opponent)
    {
        if(onReceiveDmgEvent != null)
        {
            onReceiveDmgEvent(this, opponent.GetComponent<Unit>());
        }
    }

    //deploy method spawns the character onto map
    public abstract GameObject deploy();

    //planned for hightlight effect when mouse hovering.
    protected abstract void OnMouseOver();

    protected abstract void OnMouseExit();

    //click and drag to move character or when in special phase to active skills;
    protected virtual void OnMouseDown()
    {
        if (!isMoving && LevelController.levelController.roundStatus.Equals(RoundStatus.move) && !hasMoved && isTurn)
        {
            onUpdateEvent -= snapToFloor;
            onUpdateEvent += move;
            isMoving = true;
            MoveEvent();
        }
    }

    //to active skill in special phase;
    protected virtual void OnMouseUp()
    {
        if (LevelController.levelController.roundStatus.Equals(RoundStatus.specialPhase) && cd <= 0)
        {
            foreach (Skills s in skills)
            {
                if (!s.isPassive)
                {
                    s.effect();
                    cd = s.cd;
                }
            }
        }
    }

    //reset all flags when a round is over.
    public virtual void resetFlags()
    {
        hasMoved = false;
        isTurn = false;
        hasAttacked = false;
        cd--;
    }

    //move method to caculate legal movements.
    private Ray ray;
    private RaycastHit hit;
    protected virtual void move()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("floor")))
        {
            transform.position = hit.transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                if(illegalMove(hit))
                {
                    onUpdateEvent -= move;
                    onUpdateEvent += snapToFloor;
                    isMoving = false;
                    resrictionVisual();
                    return;
                }
                lastPosition = currentPosition;
                currentPosition = hit.transform.gameObject;
                if(lastPosition == currentPosition)
                {
                    hasMoved = false;
                }
                else
                {
                    hasMoved = true;
                    onUpdateEvent += chooseFacing;
                }
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                isTurn = false;
                isMoving = false;
                resrictionVisual();
                traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
                return;
            }



            if (Input.GetMouseButtonDown(1))
            {
                onUpdateEvent -= move;
                onUpdateEvent += snapToFloor;
                isMoving = false;
                resrictionVisual();
                return;
            }
        }
    }

    protected virtual void chooseFacing() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("floor"))) {
            Vector2 thisV = new Vector2(0, 1);
            Vector2 hitV = new Vector2(hit.transform.position.x - transform.position.x, hit.transform.position.z - transform.position.z);
            float angle = Vector2.Angle(thisV, hitV);
            switch (angle) {
                case 0f:
                    leftArrow.SetActive(false);
                    upArrow.SetActive(false);
                    downArrow.SetActive(false);
                    rightArrow.SetActive(true);
                    sprite.GetComponent<SpriteRenderer>().flipX = false;
                    Status.facing = facing.right;
                    break;
                case 180f:
                    leftArrow.SetActive(true);
                    upArrow.SetActive(false);
                    downArrow.SetActive(false);
                    rightArrow.SetActive(false);
                    sprite.GetComponent<SpriteRenderer>().flipX = true;
                    Status.facing = facing.left;
                    break;
                case 90f:
                    if(hit.transform.position.x - transform.position.x >= 0)
                    {
                        leftArrow.SetActive(false);
                        upArrow.SetActive(false);
                        downArrow.SetActive(true);
                        rightArrow.SetActive(false);
                        Status.facing = facing.down;
                    }
                    else
                    {
                        leftArrow.SetActive(false);
                        upArrow.SetActive(true);
                        downArrow.SetActive(false);
                        rightArrow.SetActive(false);
                        Status.facing = facing.up;
                    }
                    break;
            }

            if (Input.GetMouseButtonUp(0))
            {
                onUpdateEvent -= chooseFacing;
                leftArrow.SetActive(false);
                upArrow.SetActive(false);
                downArrow.SetActive(false);
                rightArrow.SetActive(false);
                LevelController.levelController.startMove();
            }
        }
    }

    protected float AngleBettween(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    //change floor color to indicate legal movement. only used for controlleable units(allies)
    protected virtual void resrictionVisual()
    {
        GameObject tempHolder = GameObject.Find("tempHolder");
        if (isMoving)
        {
            foreach(GameObject x in viableRoutes)
            {
                GameObject indicator = Instantiate<GameObject>(indicatorFloor, tempHolder.transform);
                indicator.transform.position = x.transform.position + new Vector3(0, 0.01f, 0);
            }
        }
        else
        {
            foreach(Transform x in tempHolder.transform)
            {
                Destroy(x.gameObject);
            }
        }
        return;
    }

    //check if current ray hit is leagal to move.
    protected virtual bool illegalMove(RaycastHit ray)
    {
        GameObject theGrid = ray.transform.gameObject;
        if(!viableRoutes.Contains(theGrid))
        {
            return true;
        }

        foreach(Unit x in LevelController.levelController.aliveUnits)
        {
            if(x.currentPosition == theGrid)
            {
                return true;
            }
        }
        return false;
    }

    //caculate viable grids for movement. updates after every movement.
    protected virtual List<GameObject> traceViableGrids(GameObject[,,] matrix)
    {
        List<GameObject> viable = new List<GameObject>();
        int[] currentIndex = BattleGridsGen.returnMatrixIndex(currentPosition);

        for (int i = 0; i < BattleGridsGen.battleGridsGen.col; i++)
        {
            for(int j = 0; j < BattleGridsGen.battleGridsGen.row; j++)
            {
                if((Math.Abs((i - currentIndex[0])) + Math.Abs((j - currentIndex[1]))) <= moveRange)
                {
                    viable.Add(matrix[i, j, 0]);
                }
            }
        }
        viableRoutes = viable;
        return viable;
    }

    //attacks.
    public virtual void attack(GameObject target)
    {
        if (hasAttacked) { return; }
        attackEvent(target);
        target.GetComponent<Unit>().receiveDmg(gameObject);
        hasAttacked = true;
    }


    //replaced with simulateAttackResult in LevelController.
    public virtual status simulateReceiveDmg(GameObject opponent)
    {
        status opponentStatus = new status(opponent.GetComponent<Unit>().Status);
        status selfStatus = new status(this.Status);
        int dmg = opponentStatus.atk;
        if (selfStatus.isBack(opponent)) { dmg = dmg * 2; }

        if (opponentStatus.isMagic)
        {
            selfStatus.hp = selfStatus.hp - (dmg * selfStatus.mr);
        }
        else
        {
            if (selfStatus.def >= dmg)
            {
                selfStatus.hp -= 1;
            }
            else
            {
                selfStatus.hp = selfStatus.hp - (dmg - selfStatus.def);
            }
        }
        return opponentStatus;
    }

    //receives dmg.
    public virtual status receiveDmg(GameObject opponent)
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


        if (isDead()) 
        {
            LevelController.levelController.aliveUnits.Remove(this);
            Destroy(this.gameObject); 
        }
        return this.Status;
    }

    //calculate two units' distance of each other.
    protected virtual int calculateDistance(Unit unit1, Unit unit2)
    {
        int[] unit1P = BattleGridsGen.returnMatrixIndex(unit1.currentPosition);
        int[] unit2P = BattleGridsGen.returnMatrixIndex(unit2.currentPosition);
        return Mathf.Abs(unit1P[0] - unit2P[0]) + Mathf.Abs(unit1P[1] - unit2P[1]);
    }

    //calculate two grids' distance.
    protected virtual int calculateDistance(GameObject grid1, GameObject grid2)
    {
        int[] unit1P = BattleGridsGen.returnMatrixIndex(grid1);
        int[] unit2P = BattleGridsGen.returnMatrixIndex(grid2);
        return Mathf.Abs(unit1P[0] - unit2P[0]) + Mathf.Abs(unit1P[1] - unit2P[1]);
    }

    protected virtual GameObject caculateClosestGridTo(Unit unit)
    {
        int distance = BattleGridsGen.battleGridsGen.col * BattleGridsGen.battleGridsGen.row;
        GameObject closestGrid = null;
        foreach (GameObject x in viableRoutes)
        {
            int temp = calculateDistance(unit.currentPosition, x);
            if (temp < distance && temp != 0)
            {
                closestGrid = x;
                distance = temp;
            }
            else
            {
                continue;
            }
        }
        return closestGrid;
    }

    protected virtual GameObject caculateFarthestGridTo(Unit unit)
    {
        int distance = 0;
        List<GameObject> fartherestGrid = new List<GameObject>();
        foreach (GameObject x in viableRoutes)
        {
            int temp = calculateDistance(unit.currentPosition, x);
            if (temp >= distance && temp != 0)
            {
                fartherestGrid.Add(x);
                distance = temp;
            }
            else
            {
                continue;
            }
        }
        System.Random rnd = new System.Random();
        return fartherestGrid[rnd.Next(fartherestGrid.Count)];
    }

    float timer = 0f;
    //move toward a unit.
    protected virtual void moveTowardCurr()
    {
        timer += Time.deltaTime / 1f;
        if(timer >= 0.1f)
        {
            transform.position = Vector3.Lerp(lastPosition.transform.position, currentPosition.transform.position, timer);
        }
        if(timer >= 1.1f)
        {
            sprite.GetComponent<Animator>().SetBool("isMoving", false);
            onUpdateEvent -= moveTowardCurr;
            onUpdateEvent += snapToFloor;
            hasMoved = true;
            LevelController.levelController.startMove();
        }
    }

    protected virtual void startMoveToward(Unit unit)
    {
        onUpdateEvent -= snapToFloor;
        lastPosition = currentPosition;
        currentPosition = caculateClosestGridTo(unit);
        sprite.GetComponent<Animator>().SetBool("isMoving", true);
        timer = 0f;
        onUpdateEvent += moveTowardCurr;
    }

    protected virtual void startMoveAway(Unit unit)
    {
        onUpdateEvent -= snapToFloor;
        lastPosition = currentPosition;
        currentPosition = caculateFarthestGridTo(unit);
        sprite.GetComponent<Animator>().SetBool("isMoving", true);
        timer = 0f;
        onUpdateEvent += moveTowardCurr;
    }


    //make character snap to grid. called every updates.
    public virtual void snapToFloor()
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

    //helper method to make sure after every move phase that a unit's hasmoved flag is true;
    public void changeMoveFlag()
    {
        hasMoved = true;
    }

    public virtual void countEffectPeriod()
    {
        foreach(Effect e in Status.effects.ToList())
        {
            e.period--;
            if(e.period <= 0)
            {
                e.remove();
            }
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        maincamera = Camera.main;
        LevelController.levelController.onMoveEndEvent += changeMoveFlag;
        LevelController.levelController.onSpecialPhaseEndEvent += countEffectPeriod;
        onUpdateEvent += snapToFloor;
        onMoveEvent += resrictionVisual;
        traceViableGrids(BattleGridsGen.battleGridsGen.gridMatrix);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateEvent();
    }

    private void Awake()
    {
        maincamera = Camera.main;
    }
}
