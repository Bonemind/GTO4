using UnityEngine;
using System.Collections.Generic;

public class Unit : Photon.MonoBehaviour {
    /// <summary>
    /// The number of steps this unit can make in a turn
    /// </summary>
    public int StepCount = 2;

    /// <summary>
    /// The number of attacks this unit can do in a turn
    /// </summary>
    public int AttackCount = 1;

    /// <summary>
    /// The range this unit can attack in
    /// </summary>
    public int AttackRange = 1;

    /// <summary>
    /// The number of turns it takes to build this unit
    /// </summary>
    public int BuildTurns = 3;

    /// <summary>
    /// The cost for resource 1
    /// </summary>
    public int CostRes1 = 10;

    /// <summary>
    /// The cost for resource 2
    /// </summary>
    public int CostRes2 = 5;

    /// <summary>
    /// The cost for resource 3
    /// </summary>
    public int CostRes3 = 10;

    /// <summary>
    /// The damage this unit can deal per attack
    /// </summary>
    public int Damage = 10;

    /// <summary>
    /// The number of steps this unit has left
    /// </summary>
    private int stepsLeft;

    /// <summary>
    /// The number of attacks this unit has left
    /// </summary>
    private int attacksLeft;

    /// <summary>
    /// The location of the current object
    /// </summary>
    private BoardLocation boardLocation;

    /// <summary>
    /// The movement speed of this object
    /// </summary>
    public float MovementSpeed = 2f;

    /// <summary>
    /// The list of walkable tiles
    /// </summary>
    private List<GameObject> walkableTiles;

    /// <summary>
    /// The list of tiles this unit can attack
    /// </summary>
    private List<GameObject> attackableTiles;

    /// <summary>
    /// The path left to travel by this unit
    /// </summary>
    private List<GameObject> currentPath;

	/// <summary>
	/// Initialization
	/// </summary>
	public void Start () {
        stepsLeft = StepCount;
        attacksLeft = AttackCount;
        boardLocation = gameObject.GetComponent<BoardLocation>();
        walkableTiles = new List<GameObject>();
        attackableTiles = new List<GameObject>();
        currentPath = new List<GameObject>();
	}

    /// <summary>
    /// Handles starting of a turn
    /// </summary>
    public void TurnStart()
    {
        if (photonView.isMine)
        {
            stepsLeft = StepCount;
            attacksLeft = AttackCount;
        }
    }

    /// <summary>
    /// Update method
    /// </summary>
    public void Update()
    {
        if (currentPath.Count == 0)
        {
            return;
        }
        if (currentPath[0].transform.position == gameObject.transform.position)
        {
            currentPath.Remove(currentPath[0]);
        }
        if (currentPath.Count == 0)
        {
            return;
        }
        Vector3 target = currentPath[0].transform.position;
        float step = MovementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    /// <summary>
    /// Mouse hovering over object
    /// </summary>
    /// <param name="location">The location this event occured</param>
    public void MouseEnter(LocationStruct location)
    {
    }

    /// <summary>
    /// A left click on an object
    /// </summary>
    /// <param name="location">The location this occured</param>
    public void LeftClick(LocationStruct location)
    {

    }

    /// <summary>
    /// Middle click on an object
    /// </summary>
    /// <param name="location">The location this occured</param>
    public void MiddleClick(LocationStruct location)
    {
    }

    /// <summary>
    /// Right click on an object
    /// </summary>
    /// <param name="location">The location this occured</param>
    public void RightClick(LocationStruct location)
    {
        if (walkableTiles.Contains(Board.GetTileFromLocation(location)))
        {
            currentPath = Board.GetPathToLocation(boardLocation.location, location, stepsLeft);
            boardLocation.SetLocation(location.row, location.column);
            stepsLeft -= currentPath.Count;
        }
        else if (attackableTiles.Contains(Board.GetTileFromLocation(location)))
        {
            TileControl target = Board.GetTileControlFromLocation(location);
            GameObject targetObject = target.occupyingObject;
            if (targetObject == null)
            {
                return;
            }
            targetObject.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Sentmessage");
            attacksLeft--;
            stepsLeft = 0;
        }
        SetWalkableTilesHighlight(false);
        UpdateWalkableObjects();
        
    }

    /// <summary>
    /// Handles selecting of this object
    /// </summary>
    /// <param name="selected">Whether we are selected or deselected</param>
    public void Selection(bool selected)
    {
        if (selected)
        {
            UpdateWalkableObjects();
            UpdateAttackableObjects();
            SetWalkableTilesHighlight(true);
        }
        else
        {
            SetWalkableTilesHighlight(false);
        }
    }

    private void UpdateWalkableObjects()
    {
        walkableTiles.Clear();
        Board.GetWalkableTiles(walkableTiles, Board.board[boardLocation.location.row, boardLocation.location.column], stepsLeft);
    }

    private void UpdateAttackableObjects()
    {
        attackableTiles.Clear();
        Board.GetAttackableTiles(attackableTiles, Board.board[boardLocation.location.row, boardLocation.location.column], stepsLeft);
    }

    private void SetWalkableTilesHighlight(bool highlight)
    {
        string action = "Highlight";
        if (!highlight)
        {
            action = "UnHighlight";
        }
        foreach (GameObject tile in walkableTiles)
        {
            tile.SendMessage(action, SendMessageOptions.DontRequireReceiver);
        }
    }
}
