using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Shited.MatchMessages;
using System;

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

    public KupaGUI kupaGUI;
    public PileGUI pileGUI;
    public Deck pile;

    public static int players_spawn = 0;
    public Button playerReadyBtn;

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

    public override void OnStartClient()
    {
        base.OnStartClient();
        kupaGUI.FillMatchCards();
        //Debug.Log("match cards: "+MatchCards.Count);
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
        //Debug.Log("ready called  " + localPlayer.connectionToClient);
        //Debug.Log("not server");
        CmdPlayerReady(localPlayer.connectionToClient);
        //localPlayer.PlayerReady();
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayerReady(NetworkConnectionToClient playerConn)
    {
        //Debug.Log("Cmd player ready called");
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
                break;
            }
        }
    }

    public void MakePlay()
    {
        CmdMakePlay();
        //localPlayer.handGui.DisableDeck();
    }

    [Command(requiresAuthority = false)]
    public void CmdMakePlay()
    {
        switch (GameState)
        {
            //case START:
            //    boolean players_ready = true;
            //    System.out.println(state);
            //    for (Player player : players)
            //        players_ready &= player.SetSelectedAsOpenCards();
            //    if (players_ready)
            //    {
            //        ChooseStartingPlayer();
            //        players.get(current_player).SetRelevantCards(pile.GetPileTopCard());
            //        SetGameState(GameStates.GAMEPLAY);
            //    }
                //System.out.println(state);
                //break;
            case GameStates.GAMEPLAY:
                bool extra_turn = false;
                foreach (int index in currentPlayer.selected_cards)
                {
                    Card c = cards[index].card;
                    CardTransaction(currentPlayer, c);
                }
                if (kupaGUI.kupa.isEmpty())
                {
                    if (currentPlayer.getHand().GetDeckSize() < 7)
                        currentPlayer.UseOpenCards();
                    else if (currentPlayer.getHand().GetDeckSize() < 4)
                        currentPlayer.UseFaceDownCards();
                }
                //currentPlayer.selected_cards.Clear();
                Card topcard = pile.Peak();
                switch (topcard.getValue())
                {
                    case 8:// Skip next player
                        Debug.Log("skip next player");
                        NextPlayer();
                        break;
                    case 10:        // Clear pile and extra turn
                        Debug.Log("burn pile");
                        extra_turn = true;
                        //				System.out.println("Pile Before Clear:");
                        pile.clearDeck();
                        RpcBurnPile();
                        //pilePanel.ClearPanel();
                        currentPlayer.getHand().ClearSelection();
                        //refresh pile for players
                        SetCurrentPlayer(players.IndexOf(currentPlayer));
                        currentPlayer.SetRelevantCards(new Card(0,0,0));
                        break;
                }
                if (!extra_turn)
                    NextPlayer();
                bool isPlayable = true;
                if (!pile.isEmpty())
                    isPlayable = currentPlayer.SetRelevantCards(pile.GetPileTopCard());
                if (!isPlayable)
                {
                    Debug.Log("isPlayable: " + isPlayable);
                    //System.out.println(GetCurrentPlayer().getName() + " got the pile: " + pile.AsString());
                    //currentPlayer.getHand().AddDeck(pile);
                    Debug.Log("pile: " + pile.GetDeckSize());
                    RpcPlayerTakePile(currentPlayer, pile.GetDeckIndexs());
                    currentPlayer.getHand().AddDeck(pile);
                    //pile.clearDeck();
                    //currentPlayer.SetRelevantCards(new Card(0,0,0));
                    //pilePanel.ShowDeck();
                    //CardTransaction(currentPlayer, currentPlayer.getHand().getLowestCard());
                    //NextPlayer();
                }
                break;
        }
        if (GameState == GameStates.GAMEOVER)
        {
            Debug.Log("game over");
            //statuslbl.setText("Player: " + GetCurrentPlayer().getName() + "won ido the smartes guy in the room" + "Hey. yey. wow. yesh. kef. mmm.");
        }
    }

    [ClientRpc]
    public void RpcBurnPile()
    {
        pileGUI.ClearPile();
    }

    [ClientRpc]
    public void RpcStopNextPlayer()
    {
        Debug.Log("stop player");
    }

    [ClientRpc]
    public void RpcPlayerTakePile(MatchPlayer player, List<int> pile_indexs)
    {
        player.TakePile(pile_indexs);
        pileGUI.pile_cards.Clear();
    }

    private void SetCurrentPlayer(Index current_player_index)
    {
        currentPlayer = players[current_player_index];
        currentPlayer.selected_cards.Clear();
    }
    public void NextPlayer()
    {
        currentPlayer.TargetDisableDeck(currentPlayer.connectionToClient);
        int nextplayer = players.IndexOf(currentPlayer);
        nextplayer++;
        if (nextplayer >= players.Count)
            nextplayer = 0;
        SetCurrentPlayer(nextplayer);
    }

    public void StartGame()
    {
        GameState = GameStates.GAMEPLAY;
        CardTransaction(currentPlayer, currentPlayer.playerHand.getLowestCard());
        NextPlayer();
        currentPlayer.SetRelevantCards(pile.GetPileTopCard());
    }



    [Server]
    public void CardTransaction(MatchPlayer p, Card c)
    {
        Debug.Log("Card trans");
        Card from_kupa;
        pile.PushCard(c);
        p.playerHand.PopCard(c);
        RpcShowCardTransaction(p, c.GetCardIndex());
        if (p.playerHand.GetDeckSize() < 9 && !kupaGUI.kupa.isEmpty())
        {
            from_kupa = kupaGUI.GetCardFromKupa().card;
            //trans_string += ", got " + from_kupa.getValue() + " from Kupa.";
            p.playerHand.PushCard(from_kupa);
            p.RpcGetCardFromKupa(from_kupa.GetCardIndex());
            //RpcGetCardFromKupa(p, from_kupa.GetCardIndex());
        }
        if(p.playerHand.GetDeckSize() == 0)
            GameState = GameStates.GAMEOVER;
        
    }

    //Must work with network connection to client?
    [ClientRpc]
    public void RpcShowCardTransaction(MatchPlayer p, int oldCard_index)
    {
        //MatchPlayer p = players_connection[];
        //Debug.Log(p.handGui);
        CardGUI card = cards[oldCard_index];
        card.CardReset();
        pileGUI.AddCardToPile(card);
        p.handGui.RemoveCard(card);

        //if (newCard_index != -1)
        //    p.handGui.PushCard(GetCardByIndex(newCard_index));
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
    public void UpdateGameUI(MatchPlayer oldPlayer, MatchPlayer newPlayerTurn)
    {
        if (!newPlayerTurn) return;

        if (newPlayerTurn.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            //Debug.Log("my turn");
            //gameText.text = "Your Turn";
            //gameText.color = Color.blue;
        }
        else
        {
            //Debug.Log("not my turn");
            //localPlayer.handGui.DisableDeck();
            //gameText.text = "Their Turn";
            //gameText.color = Color.red;
        }
    }
}
