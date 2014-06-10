using UnityEngine;
using System.Collections;

public class CommandConsoleRouter : MonoBehaviour {
    /// <summary>
    /// The commands respository
    /// </summary>
    ConsoleCommandsRepository repo;

    /// <summary>
    /// The logger to use
    /// </summary>
    ConsoleLog logger;

	/// <summary>
	/// General initialization
	/// </summary>
	public void Start () {
        repo = ConsoleCommandsRepository.Instance;
        logger = ConsoleLog.Instance;
        repo.RegisterCommand("Resources.Max", MaxResources);
        repo.RegisterCommand("Resources.Reset", ResetResources);
        repo.RegisterCommand("Resources.GetForCurrent", GetCurrentResources);
        repo.RegisterCommand("Network.ListPlayers", ListPlayers);
        repo.RegisterCommand("Network.ConnectionStatus", ConnectionStatus);
        repo.RegisterCommand("Game.PrintBoard", PrintBoard);
        repo.RegisterCommand("Game.PrintStatus", PrintStatus);
	}

    /// <summary>
    /// Increases the resources of a player to a ludicrous amount
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string MaxResources(params string[] args)
    {
        HUD hud = Camera.main.GetComponent<HUD>();
        GameResources res = hud.GetResources();
        res.SetResource(GameResources.ResourceTypes.RES1, 999999);
        res.SetResource(GameResources.ResourceTypes.RES2, 999999);
        res.SetResource(GameResources.ResourceTypes.RES3, 999999);
        return "Resources increased";
    }

    /// <summary>
    /// Sets the resources of a player to 0
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string ResetResources(params string[] args)
    {
        HUD hud = Camera.main.GetComponent<HUD>();
        GameResources res = hud.GetResources();
        res.SetResource(GameResources.ResourceTypes.RES1, 0);
        res.SetResource(GameResources.ResourceTypes.RES2, 0);
        res.SetResource(GameResources.ResourceTypes.RES3, 0);
        return "Resources reset";
    }

    /// <summary>
    /// Lists all connected players
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string ListPlayers(params string[] args)
    {
        string ret = "";
        PhotonPlayer[] players = PhotonNetwork.playerList;
        ret = "Connected players: " + players.Length.ToString() + "\r\n";
        ret += "ID : Username : MasterClient\r\n";
        foreach (PhotonPlayer player in players)
        {
            ret += string.Format("{0} : {1}  Master: {2}\r\n", player.ID.ToString(), player.name.ToString(), player.isMasterClient.ToString());
        }
        return ret;
    }

    /// <summary>
    /// Displays the resources required for the selected object as well as the current amount of resources
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string GetCurrentResources(params string[] args)
    {
        if (HUD.currentObject == null)
        {
            return "You have to select a building for this to work";
        }
        HUD hud = Camera.main.GetComponent<HUD>();
        GameResources resources = hud.GetResources();
        GameObject currentObject = HUD.currentObject;
        Building building = currentObject.GetComponent<Building>();
        string ret = string.Format("Costs: {0} {1} {2}\r\n", building.CostRes1, building.CostRes2, building.CostRes3);
        ret += string.Format("Current resources: {0} {1} {2}\r\n", resources.GetResource(GameResources.ResourceTypes.RES1), resources.GetResource(GameResources.ResourceTypes.RES2), resources.GetResource(GameResources.ResourceTypes.RES3));
        ret += string.Format("Output of checkcost: {0}\r\n", building.CheckCost());
        return ret;
    }

    /// <summary>
    /// Displays the connection status
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public string ConnectionStatus(params string[] args)
    {
        return string.Format("Status: {0} Players: {1} Ping: {2} My ID: {3}", PhotonNetwork.connectionStateDetailed.ToString(), PhotonNetwork.playerList.Length, PhotonNetwork.GetPing().ToString(), PhotonNetwork.player.ID.ToString());
    }

    public string PrintBoard(params string[] args)
    {
        string ret = "";
        for (int i = 0; i < Board.boardDimensions; i++)
        {
            string line = "";
            for (int j = 0; j < Board.boardDimensions; j++)
            {
                if (Board.board[i, j] == null)
                {
                    line += "[ ]";
                }
                else
                {
                    line += "[x]";
                }

            }
            line += "\r\n";
            ret += line;
        }
        return ret;
    }

    public string PrintStatus(params string[] args)
    {
        string ret = "";
        ret = "Status:" + HUD.currState.ToString() + "\n";
        string currObject = "null";
        if (HUD.currentObject != null)
        {
            currObject = HUD.currentObject.ToString();
        }
        ret += "currObject:" + currObject;
        return ret;
    }
}
