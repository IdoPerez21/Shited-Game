using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Shited.MatchMessages;
public class MatchController : NetworkBehaviour
{
    [SerializeReference]
    public static readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public static readonly List<CardGUI> cards = new List<CardGUI>();

    public List<PlayerHandGUI> HandViews;
    public PlayerHandGUI PlayerView;

    [SerializeReference]
    public Dictionary<NetworkConnectionToClient, MatchPlayer> players_connection = new Dictionary<NetworkConnectionToClient, MatchPlayer>();
    //public Dictionary<NetworkIdentity, MatchPlayerData> playersData = new Dictionary<NetworkIdentity, MatchPlayerData>();

    //public List<NetworkIdentity> players = new();
    public NetworkIdentity StartingPlayer;
    public KupaGUI kupaGUI;

    public PileGUI pileGUI;
    public Deck pile;

    public static int players_spawn = 0;

    public Button playerReadyBtn;
    //public NetworkIdentity myPlayer;

    public SyncList<MatchPlayer> players = new();
    public MatchPlayer localPlayer;

    [SyncVar(hook = nameof(UpdateGameUI))]
    public MatchPlayer currentPlayer;

    public int players_ready = 0;

    public enum GameStates
    {
        START,
        GAMEPLAY,
        ENDGAME,
        GAMEOVER
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
        {
            //players_connection.Add(player.connectionToClient, player);
            if (player.isLocalPlayer)
                localPlayer = player;
            SetHand(player);
        }
        if (isServer)
        {
            pile = new Deck();
            InitPlayers();
            GameState = GameStates.START;
        }
    }

    [Server]
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
        Debug.Log("ready called  " + localPlayer.connectionToClient);
        //Debug.Log("not server");
        CmdPlayerReady(localPlayer.connectionToClient);
        //localPlayer.PlayerReady();
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayerReady(NetworkConnectionToClient playerConn)
    {
        Debug.Log("Cmd player ready called");
        foreach (MatchPlayer player in players)
        {
            if (player.connectionToClient == playerConn)
            {
                player.SetSelectedAsOpenCards();
                if (players_ready == 0)
                    currentPlayer = player;
                players_ready++;
                if (players_ready == players.Count)
                    StartGame();
            }
        }
    }

    public void StartGame()
    {
        GameState = GameStates.GAMEPLAY;
        CardTransaction(currentPlayer, GetCardByIndex(currentPlayer.playerHand.getLowestCard().GetCardIndex()));
    }

    [Server]
    public void CardTransaction(MatchPlayer p, CardGUI c)
    {
        CardGUI from_kupa = null;
        pile.PushCard(c.card);
        p.playerHand.PopCard(c.card);
        RpcShowCardTransaction(p, c.card.GetCardIndex());
        if (p.playerHand.GetDeckSize() < 9 && !kupaGUI.kupa.isEmpty())
        {
            from_kupa = kupaGUI.GetCardFromKupa();
            //trans_string += ", got " + from_kupa.getValue() + " from Kupa.";
            p.playerHand.PushCard(from_kupa.card);
            RpcGetCardFromKupa(p, from_kupa.card.GetCardIndex());
        }
        if(p.playerHand.GetDeckSize() == 0)
            GameState = GameStates.GAMEOVER;
        
    }

    //Must work with network connectio nto client
    [ClientRpc]
    public void RpcShowCardTransaction(MatchPlayer p, int oldCard_index)
    {
        //MatchPlayer p = players_connection[];
        Debug.Log(p.handGui);
        CardGUI card = cards[oldCard_index];
        card.CardReset();
        pileGUI.AddCardToPile(card);
        p.handGui.RemoveCard(card);


        //if (newCard_index != -1)
        //    p.handGui.PushCard(GetCardByIndex(newCard_index));
    }

    //same
    [ClientRpc]
    public void RpcGetCardFromKupa(MatchPlayer p, int index)
    {
        Debug.Log(p.handGui);
        //MatchPlayer p = players_connection[conn.identity.connectionToClient];
        p.PushCard(cards[index]);
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayerReady()
    {
        Debug.Log("Cmd player ready called without connection");
        //foreach(MatchPlayer player in players)
        //{
        //    if (player.connectionToClient == playerConn)
        //        player.SetSelectedAsOpenCards();
        //}
    }

    public static CardGUI GetCardByIndex(int index)
    {
        return cards[index];
    }

    public static List<CardGUI> GetCardsListByIndexs(List<int> index)
    {
        List<CardGUI> list = new List<CardGUI>();
        foreach(int i in index)
            list.Add(cards[i]);
        return list;
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
