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
            Debug.Log("Called");
            OnStartMatch();
            gameObject.SetActive(false);
        });
    }

    public void OnStartMatch()
    {
        GameObject matchControllerObject = Instantiate(MatchControllerPrefab);
        //matchControllerObject.GetComponent<NetworkMatch>().matchId = matchId;

        MatchController matchController = matchControllerObject.GetComponent<MatchController>();
        int index = 0, view = 0;
        foreach (NetworkConnectionToClient playerConn in NetworkServer.connections.Values)
        {
            //playerConn.Send(new ClientMatchMessage { clientMatchOperation = ClientMatchOperation.Started });
            Debug.Log("conn id: " + playerConn.connectionId);
            GameObject player = Instantiate(NetworkManager.singleton.playerPrefab);
            MatchPlayer matchPlayer = player.GetComponent<MatchPlayer>();
            matchPlayer.matchController = matchController;
            matchPlayer.index = index;
            matchController.game_players.Add(matchPlayer);
            //matchPlayer.handGui = matchController.HandViews[index];
            //matchPlayer.matchController = matchController;

            //Debug.Log(index);

            //matchController.player = player.GetComponent<MatchPlayer>();
            //player.GetComponent<NetworkMatch>().matchId = matchId;
            NetworkServer.AddPlayerForConnection(playerConn, player);
            NetworkServer.Spawn(player, playerConn);
            
            if (matchPlayer.hasAuthority)
            {
                Debug.Log("signing match player");
                matchController.player = matchPlayer;
            }
            //else
            //{
            //    matchController.PlayerHands.Add(playerConn, matchController.HandViews[view]);
            //    view++;
            //}
            //matchController.players.Add(playerConn.identity);
            index++;
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
