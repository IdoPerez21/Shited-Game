using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Shited.MatchMessages;
using UnityEngine.UI;
public class CanvasController : MonoBehaviour
{
    internal static Dictionary<NetworkConnectionToClient, PlayerInfo> playersInfo = new Dictionary<NetworkConnectionToClient, PlayerInfo>();

    public GameObject MatchControllerPrefab;
    public Button StartBtn;

    void Start()
    {
        StartBtn.onClick.AddListener(() =>
        {
            OnStartMatch();
            gameObject.SetActive(false);
        });
    }

    public void OnStartMatch()
    {
        GameObject matchControllerObject = Instantiate(MatchControllerPrefab);
        //matchControllerObject.GetComponent<NetworkMatch>().matchId = matchId;

        MatchController matchController = matchControllerObject.GetComponent<MatchController>();
        int index = 0;
        foreach (NetworkConnectionToClient playerConn in NetworkServer.connections.Values)
        {
            //playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Started });

            GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
            player.GetComponent<MatchPlayer>().index = index;
            index++;
            Debug.Log(index);
            matchController.player = player.GetComponent<MatchPlayer>();
            //player.GetComponent<NetworkMatch>().matchId = matchId;
            NetworkServer.AddPlayerForConnection(playerConn, player);
            NetworkServer.Spawn(player, playerConn);
            //matchController.players.Add(playerConn.identity);
        }
        NetworkServer.Spawn(matchControllerObject);

        //matchController.startingPlayer = matchController.player1;
        //matchController.currentPlayer = matchController.player1;
    }

    internal void OnServerReady(NetworkConnectionToClient conn)
    {

    }
    // Start is called before the first frame update
}
