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

    private BoardLocation boardLocation;

    public float MovementSpeed = 2f;

    private List<GameObject> walkableTiles;

	/// <summary>
	/// Initialization
	/// </summary>
	public void Start () {
        stepsLeft = StepCount;
        boardLocation = gameObject.GetComponent<BoardLocation>();
        walkableTiles = new List<GameObject>();
	}

    /// <summary>
    /// Handles starting of a turn
    /// </summary>
    public void TurnStart()
    {
        if (photonView.isMine)
        {
            stepsLeft = StepCount;
        }
    }

    public void Update()
    {
        Vector3 target = Board.GetTileFromLocation(boardLocation.location).transform.position;
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
            boardLocation.SetLocation(location.row, location.column);
            stepsLeft = 0;
            SetWalkableTilesHighlight(false);
            UpdateWalkableObjects();
        }
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
