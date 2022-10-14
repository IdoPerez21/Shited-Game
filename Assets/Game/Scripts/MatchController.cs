using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Shited.MatchMessages;
public class MatchController : NetworkBehaviour
{
    [SerializeReference]
    internal readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public List<PlayerHandGUI> HandViewList;
    public PlayerHandGUI PlayerView;

    [SerializeReference]
    public Dictionary<NetworkConnectionToClient, PlayerHandGUI> PlayerHands = new Dictionary<NetworkConnectionToClient, PlayerHandGUI>();
    //public Dictionary<NetworkIdentity, MatchPlayerData> playersData = new Dictionary<NetworkIdentity, MatchPlayerData>();

    public List<NetworkIdentity> players = new List<NetworkIdentity>();
    public NetworkIdentity StartingPlayer;
    public KupaGUI kupaGUI;
    //public NetworkIdentity myPlayer;

    public MatchPlayer player;

    public override void OnStartServer()
    {
        base.OnStartServer();
        kupaGUI.FillMatchCards();
        CmdInitPlayer();
        //NetworkServer.Spawn(NetworkManager.singleton.playerPrefab)
        Debug.Log(player.connectionToClient);
        //NetworkServer.Spawn(player.gameObject, connectionToClient);
        Debug.Log(MatchCards.Count);


    }
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();

        Debug.Log("command init players");
        //Debug.Log(PlayerHands);
        //Debug.Log("My player " + myPlayer); 
    }

    void Start()
    {
        //int index = 0;
        //foreach(NetworkIdentity identity in players)
        //{
        //    PlayerHands.Add(identity.connectionToClient, HandViewList[index].GetComponent<PlayerHandGUI>());
        //    HandViewList[index].SetActive(true);
        //    index++;
        //}
        //CmdInitPlayer();    
    }

    [Command]
    public void CmdInitPlayer()
    {
        Debug.Log("Init players");
        player.InitPlayer(kupaGUI.kupa.GetRandomDeck(9));

        //List<CardGUI> cards = kupaGUI.GetRandomDeck(9);
        Debug.Log(player.connectionToClient);
        foreach (Card card in player.playerHand.GetCardsList())
        {
            NetworkServer.Spawn(MatchCards[card].gameObject, player.connectionToClient);
            RpcShowCard(MatchCards[card], player.index);
        }
    }

    [ClientRpc]
    public void RpcShowCard(CardGUI card, int playerIndex)
    {
        Debug.Log(playerIndex);
        if (card.hasAuthority)
            PlayerView.ShowCard(card);
        else
            HandViewList[playerIndex].ShowCard(card);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
