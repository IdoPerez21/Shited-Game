using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Shited.MatchMessages;
public class MatchController : NetworkBehaviour
{
    [SerializeReference]
    public static readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public static readonly List<CardGUI> cards = new List<CardGUI>();

    public List<PlayerHandGUI> HandViews;
    public PlayerHandGUI PlayerView;

    [SerializeReference]
    public Dictionary<NetworkConnectionToClient, PlayerHandGUI> PlayerHands = new Dictionary<NetworkConnectionToClient, PlayerHandGUI>();
    //public Dictionary<NetworkIdentity, MatchPlayerData> playersData = new Dictionary<NetworkIdentity, MatchPlayerData>();

    //public List<NetworkIdentity> players = new();
    public NetworkIdentity StartingPlayer;
    public KupaGUI kupaGUI;

    public static int players_spawn = 0;
    //public NetworkIdentity myPlayer;

    public SyncList<MatchPlayer> players = new();

    [SyncVar(hook = nameof(UpdateGameUI))]
    public MatchPlayer currentPlayer;

    public enum GameStates
    {
        START,
        GAMEPLAY,
        ENDGAME
    }

    public static GameStates GameState;
    //public MatchPlayer player;

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        //currentPlayer.OpenCard();

        //CmdInitPlayer();
        //NetworkServer.Spawn(NetworkManager.singleton.playerPrefab)
        //Debug.Log(player.connectionToClient);
        //NetworkServer.Spawn(player.gameObject, connectionToClient);
        //Debug.Log(MatchCards.Count);
    }

    [TargetRpc]
    public void TargetTest(NetworkConnection target)
    {
        Debug.Log("Target: " + target);
    }

    [ClientRpc]
    public void RpcTest()
    {
        Debug.Log("ClientRpc");
    }
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();
        kupaGUI.FillMatchCards();
        Debug.Log("match cards: "+MatchCards.Count);
        foreach (MatchPlayer player in players)
            SetHand(player);
        if (isServer)
        {
            InitPlayers();
            GameState = GameStates.START;
        }
    }

    void InitPlayers()
    {
        foreach(MatchPlayer player in players)
        {
            //player.InitPlayer(kupaGUI.GetRandomDeck(9));
            player.InitPlayer(kupaGUI.kupa.GetRandomDeck(9));
        }
    }

    public void OnClickPlayerReady()
    {

    }

    [TargetRpc]
    public void TargetOpen()
    {
        Debug.Log("open");
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

    public void SetHand(MatchPlayer player)
    {
        if (player.hasAuthority)
        {
            player.handGui = PlayerView;
        }
        else
        {
            player.handGui = HandViews[players_spawn];
            players_spawn++;
        }

        //Debug.Log(players_spawn);
    }

    //[Command(requiresAuthority = false)]
    //public void CmdInitPlayer()
    //{
    //    Debug.Log("Init player");
    //    //SetPlayerHand(matchPlayer);
    //    List<CardGUI> cards = kupaGUI.GetRandomDeck(9);
    //    player.InitPlayer(cards);
    //    foreach (CardGUI card in cards)
    //        RpcShowCard(card);
    //    ////List<CardGUI> cards = kupaGUI.GetRandomDeck(9);
    //    //Debug.Log(player.connectionToClient);
    //    //foreach (Card card in player.playerHand.GetCardsList())
    //    //{
    //    //    NetworkServer.Spawn(MatchCards[card].gameObject, player.connectionToClient);
    //    //    RpcShowCard(MatchCards[card], player.index);
    //    //}
    //}
    public void UpdateGameUI(MatchPlayer oldPlayer, MatchPlayer newPlayerTurn)
    {
        if (!newPlayerTurn) return;

        if (newPlayerTurn.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            Debug.Log("my turn");
            //gameText.text = "Your Turn";
            //gameText.color = Color.blue;
        }
        else
        {
            Debug.Log("not my turn");
            //gameText.text = "Their Turn";
            //gameText.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
