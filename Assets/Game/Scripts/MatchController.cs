using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Shited.MatchMessages;
public class MatchController : NetworkBehaviour
{
    [SerializeReference]
    internal readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public List<PlayerHandGUI> HandViews;
    public PlayerHandGUI PlayerView;

    [SerializeReference]
    public Dictionary<NetworkConnectionToClient, PlayerHandGUI> PlayerHands = new Dictionary<NetworkConnectionToClient, PlayerHandGUI>();
    //public Dictionary<NetworkIdentity, MatchPlayerData> playersData = new Dictionary<NetworkIdentity, MatchPlayerData>();

    public List<NetworkConnectionToClient> players = new();
    public NetworkIdentity StartingPlayer;
    public KupaGUI kupaGUI;

    public static int players_spawn = 0;
    //public NetworkIdentity myPlayer;
    public SyncList<MatchPlayer> game_players = new();
    public MatchPlayer player;

    public override void OnStartServer()
    {
        base.OnStartServer();
        kupaGUI.FillMatchCards();
        //CmdInitPlayer();
        //NetworkServer.Spawn(NetworkManager.singleton.playerPrefab)
        //Debug.Log(player.connectionToClient);
        //NetworkServer.Spawn(player.gameObject, connectionToClient);
        //Debug.Log(MatchCards.Count);
    }
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();
        int index = 0;
        foreach (MatchPlayer player in game_players)
        {
            Debug.Log("player connection: " + player.connectionToClient);
            player.handGui = HandViews[index];
            index++;
        }
        Debug.Log("command init players");
        CmdInitPlayer(player);
        //Debug.Log(PlayerHands.Keys);
        //foreach (MatchPlayer player in game_players)
        //{
        //    if (player.hasAuthority)
        //    {
        //        Debug.Log("Auth");
        //        player.handGui = PlayerView;
        //    }
        //    else
        //    {
        //        player.handGui = HandViews[index];
        //        index++;
        //        Debug.Log(index);
        //    }
        //}

        //Debug.Log(PlayerHands);
        //Debug.Log("My player " + myPlayer); 
    }

    void Start()
    {

        //CmdInitPlayer();
        //int index = 0;
        //foreach(NetworkIdentity identity in players)
        //{
        //    PlayerHands.Add(identity.connectionToClient, HandViews[index].GetComponent<PlayerHandGUI>());
        //    HandViews[index].SetActive(true);
        //    index++;
        //}
        //CmdInitPlayer();    
    }

    [Command(requiresAuthority = false)]
    public void CmdInitPlayer(MatchPlayer player)
    {
        Debug.Log("Init player");
        //SetPlayerHand(matchPlayer);
        player.InitPlayer(kupaGUI.GetRandomDeck(9));
        ////List<CardGUI> cards = kupaGUI.GetRandomDeck(9);
        //Debug.Log(player.connectionToClient);
        //foreach (Card card in player.playerHand.GetCardsList())
        //{
        //    NetworkServer.Spawn(MatchCards[card].gameObject, player.connectionToClient);
        //    RpcShowCard(MatchCards[card], player.index);
        //}
    }

    [ClientRpc]
    public void RpcShowCard(CardGUI card, int playerIndex)
    {
        Debug.Log(playerIndex);
        if (card.hasAuthority)
            PlayerView.ShowCard(card);
        else
            HandViews[playerIndex].ShowCard(card);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
