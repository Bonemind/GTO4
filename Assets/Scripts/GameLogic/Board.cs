using UnityEngine;
using System.Collections.Generic;

public class Board : Photon.MonoBehaviour
{
    /// <summary>
    /// Dimensions of the board
    /// This is both x and z
    /// </summary>
    public static int boardDimensions = 10;

    /// <summary>
    /// The dimensions of the tiles
    /// This is both x and z
    /// </summary>
    public float tileDemensions = 5f;

    /// <summary>
    /// The tileprefab
    /// </summary>
    public GameObject[] tilePrefabs;

    /// <summary>
    /// The board containing all tiles
    /// </summary>
    public static GameObject[,] board;

	// Use this for initialization
	void Start () {
        board = new GameObject[boardDimensions, boardDimensions];
	}
    public void OnJoinedRoom()
    {
        //Generate the board if we are the master client
        if (PhotonNetwork.isMasterClient)
        {
            generateBoardSymmetrical();
        }
    }

    /// <summary>
    /// Generates the game board
    /// </summary>
    private void generateBoard()
    {
        for (int r = 0; r < boardDimensions; r++)
        {
            for (int c = 0; c < boardDimensions; c++)
            {
                Vector3 pos = new Vector3(r * tileDemensions, 0f, c * tileDemensions);
                GameObject go = (GameObject) Instantiate(getRandomTile(), pos, new Quaternion());
                SetObjectProperties(ref go, r, c);
            }
        }
    }

    /// <summary>
    /// Generates a game board that is symmetrical to give players an equal chance
    /// </summary>
    private void generateBoardSymmetrical()
    {
        int rowCeiling = Mathf.CeilToInt(boardDimensions / 2);
        //Correct the point we will reverse copy our map on for even numbers
        if (boardDimensions % 2 == 0) {
            rowCeiling--;
        }
        for (int r = 0; r <= rowCeiling; r++)
        {
            for (int c = 0; c < boardDimensions; c++)
            {
                Vector3 pos = new Vector3(r * tileDemensions, 0f, c * tileDemensions);
                GameObject go = (GameObject)PhotonNetwork.Instantiate(getRandomTile().name, pos, new Quaternion(), 0);
                SetObjectProperties(ref go, r, c);
            }
        }

        //We've generated half the map, reverse copy the map to the other half to achieve a symmetrical map
        for (int r = boardDimensions - 1; r > rowCeiling; r--)
        {
            for (int c = 0; c < boardDimensions; c++)
            {
                Vector3 pos = new Vector3(r * tileDemensions, 0f, c * tileDemensions);
                GameObject go = (GameObject)PhotonNetwork.Instantiate(board[(boardDimensions - 1) - r, c].name.Replace("(Clone)", ""), pos, new Quaternion(), 0);
                SetObjectProperties(ref go, r, c);
            }
        }
    }

    /// <summary>
    /// Returns a random tile
    /// </summary>
    /// <returns>A tile gameobject</returns>
    private GameObject getRandomTile()
    {
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }

    /// <summary>
    /// Sets object location and boardlocation to passed parameters
    /// </summary>
    /// <param name="go">The gameobject to set the properties of</param>
    /// <param name="r">The row to set it to</param>
    /// <param name="c">The column to set it to</param>
    private void SetObjectProperties(ref GameObject go, int r, int c)
    {
        go.GetComponent<BoardLocation>().SetLocation(r, c);
        board[r, c] = go;
        go.transform.parent = transform;
    }

    /// <summary>
    /// Get the closest free tile to the current object
    /// </summary>
    /// <param name="location">The location to start at</param>
    /// <returns>The closest free tile</returns>
    public static LocationStruct? GetClosestFreeTile(LocationStruct location)
    {
        int range = 1;
        int row = location.row;
        int column = location.column;
        TileControl currTile = Board.GetTileControlFromLocation(location);
        while (range < boardDimensions)
        {
            List<GameObject> neighbourTiles = Board.neighbourTiles(location, range);
            Debug.Log("Neigbourtiles" + neighbourTiles.Count);
            foreach (GameObject go in neighbourTiles)
            {
                TileControl tc = go.GetComponent<TileControl>();
                if (tc != null)
                {
                    if (tc.occupyingObject == null)
                    {
                        return go.GetComponent<BoardLocation>().location;
                    }
                }
            }
            range++;
        }
        return null;
    }

    /// <summary>
    /// Gets the tilecontrol blonging to the tile of which the location is passed
    /// </summary>
    /// <param name="location">The location we want the Tilecontrol from</param>
    /// <returns>The tilecontrol</returns>
    public static TileControl GetTileControlFromLocation(LocationStruct location) 
    {
        return board[location.row, location.column].GetComponent<TileControl>();
    }

    public static GameObject GetTileFromLocation(LocationStruct location)
    {
        return board[location.row, location.column];
    }

    /// <summary>
    /// Checks if the tile at location is still free
    /// </summary>
    /// <param name="location">The location to check</param>
    /// <returns>True if it is, false otherwise</returns>
    public static bool IsFreeTile(LocationStruct location)
    {
        return GetTileControlFromLocation(location).occupyingObject == null;
    }

    /// <summary>
    /// Checks whther the passed tile is free
    /// overload to easily pass a row and column
    /// </summary>
    /// <param name="row">The row to check</param>
    /// <param name="column">The column to check</param>
    /// <returns>True if the tile is free, false otherwise</returns>
    public static bool IsFreeTile(int row, int column)
    {
        LocationStruct location = new LocationStruct();
        location.row = row;
        location.column = column;
        return IsFreeTile(location);
    }

    /// <summary>
    /// Returns a locationstruct from a row and column if passed location is free
    /// </summary>
    /// <param name="row">The row of the location</param>
    /// <param name="column">The column of the location</param>
    /// <returns>Locationstruct if it was free, null if it wasn't</returns>
    public static LocationStruct? GetIfFreeTile(int row, int column)
    {
        LocationStruct location = new LocationStruct();
        location.row = row;
        location.column = column;
        if (IsFreeTile(location))
        {
            return location;
        }
        return null;
    }

    /// <summary>
    /// Gets all neighbouring tiles, includes diagonal tiles
    /// </summary>
    /// <param name="currLocation">The location to start at</param>
    /// <param name="range">The range within which to look</param>
    /// <returns>List containing the neighbouring tiles</returns>
    public static List<GameObject> neighbourTiles(LocationStruct currLocation, int range)
    {
        List<GameObject> adjecantObjects = new List<GameObject>();
        int row = currLocation.row;
        int column = currLocation.column;

        //Set the range to get objects from
        int startRangeRow = (row - range < 0) ? 0 : row - range;
        int startRangeColumn = (column - range < 0) ? 0 : column - range;
        int endRangeRow = (row + range >= boardDimensions) ? boardDimensions - 1 : row + range;
        int endRangeColumn = (column + range >= boardDimensions) ? boardDimensions - 1 : column + range;

        for (int rowNum = startRangeRow; rowNum <= endRangeRow; rowNum++)
        {
            for (int colNum = startRangeColumn; colNum <= endRangeColumn; colNum++)
            {
                adjecantObjects.Add(board[rowNum, colNum]);
            }
        }
        return adjecantObjects;
    }

    /// <summary>
    /// Gets all neighbouring tiles, includes diagonal tiles
    /// Overload for a range of 1
    /// </summary>
    /// <param name="currLocation">The location to start at</param>
    /// <returns>List containing the neighbouring tiles</returns>
    public static List<GameObject> neighbourTiles(LocationStruct location)
    {
        return Board.neighbourTiles(location, 1);
    }

    /// <summary>
    /// Returns tiles that are adjecant to the current tile, but not diagonally
    /// </summary>
    /// <param name="location">The location of the queried tile</param>
    /// <returns>List of adjecant tiles</returns>
    public static List<GameObject> neighbourTilesStrait(LocationStruct location)
    {
        List<GameObject> tiles = new List<GameObject>();
        int row = location.row;
        int column = location.column;
        if (row + 1 < boardDimensions)
        {
            tiles.Add(board[row + 1, column]);
        }
        if (column + 1 < boardDimensions)
        {
            tiles.Add(board[row, column + 1]);
        }
        if (row - 1 >= 0)
        {
            tiles.Add(board[row - 1, column]);
        }
        if (column - 1 >= 0)
        {
            tiles.Add(board[row, column - 1]);
        }
        return tiles;
    }

    public static List<GameObject> GetWalkableTiles(List<GameObject> currentTiles, GameObject currTile, int stepsleft)
    {
        if (stepsleft <= 0)
        {
            return currentTiles;
        }
        stepsleft--;
        List<GameObject> adjecantTiles = neighbourTilesStrait(currTile.GetComponent<BoardLocation>().location);
        foreach (GameObject tile in adjecantTiles)
        {
            if (currentTiles.Contains(tile) || tile.GetComponent<TileControl>().occupyingObject != null)
            {
                continue;
            }
            currentTiles.Add(tile);
            if (stepsleft > 0) 
            {
                GetWalkableTiles(currentTiles, tile, stepsleft);
            }
        }
        return currentTiles;
    }

    public static Vector3 GetTileLocation(LocationStruct location)
    {
        return board[location.row, location.column].gameObject.transform.position;
    }
}
