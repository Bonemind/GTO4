using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// Dimensions of the board
    /// This is both x and z
    /// </summary>
    public int boardDimensions = 10;

    /// <summary>
    /// The dimensions of the tiles
    /// This is both x and z
    /// </summary>
    public float tileDemensions = 5f;

    /// <summary>
    /// The tileprefab
    /// </summary>
    public GameObject tilePrefab;

    /// <summary>
    /// The board containing all tiles
    /// </summary>
    private GameObject[,] board;

	// Use this for initialization
	void Start () {
        board = new GameObject[boardDimensions, boardDimensions];
        generateBoardSymmetrical();
	}
	
	// Update is called once per frame
	void Update () {
	
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
                GameObject go = (GameObject) Instantiate(tilePrefab, pos, new Quaternion());
                board[r, c] = go;
            }
        }
    }

    /// <summary>
    /// Generates a game board that is symmetrical to give players an equal chance
    /// </summary>
    private void generateBoardSymmetrical()
    {
        int rowCeiling = Mathf.CeilToInt(boardDimensions / 2);
        for (int r = 0; r <= rowCeiling; r++)
        {
            for (int c = 0; c < boardDimensions; c++)
            {
                Vector3 pos = new Vector3(r * tileDemensions, 0f, c * tileDemensions);
                GameObject go = (GameObject)Instantiate(tilePrefab, pos, new Quaternion());
                board[r, c] = go;
            }
        }
        for (int r = boardDimensions - 1; r > rowCeiling; r--)
        {
            for (int c = 0; c < boardDimensions; c++)
            {
                Vector3 pos = new Vector3(r * tileDemensions, 0f, c * tileDemensions);
                GameObject go = (GameObject)Instantiate(tilePrefab, pos, new Quaternion());
                board[r, c] = go;
            }
        }

    }
}
