using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MatchPlayer : NetworkBehaviour
{
    public PlayerHandGUI handGui;
    [SerializeReference]
    public Deck playerHand;
    public readonly SyncList<int> player_cards = new();
    public List<CardGUI> selected_cards = new();
    public int index;
    public MatchController matchController;

    void Update()
    {
       //Debug.Log("Auth: " + connectionToClient);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        //player_cards.Callback += OnDeckUpdated;
        //CmdTest();
        
        //if (isClientOnly)
        //{
        //    //Debug.Log(connectionToClient);
        //    matchController = GameObject.FindObjectOfType<MatchController>();
        //}
        //matchController.AddHand(connectionToClient);
        //Debug.Log(connectionToClient);
        //handGui = matchController.GetPlayerHand();
        //handGui = matchController.GetPlayerHand(connectionToClient);
        //if (!hasAuthority)
        //{
        //    handGui = matchController.PlayerHands[connectionToClient];
        //    Debug.Log(matchController.PlayerHands[connectionToClient]);
        //}
        //else
        //{
        //    handGui = matchController.PlayerView;
        //    Debug.Log(matchController.PlayerView);
        //}
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public void OpenCard()
    {
        Debug.Log("open card");
    }

    public void InitPlayer(List<Card> cards)
    {
        playerHand = new();
        playerHand.setCardsList(cards);
        List<int> cards_indexs = new();
        int i = 0;
        foreach (Card card in cards)
        {
            cards_indexs.Add(card.GetCardIndex());
            if (i < 3)
            {
                card.SetFace_down(true);
                i++;
            }
            //MatchController.cards[card.GetCardIndex()].UpdateUI();
            //card.Print();
            //RpcShowCard(card.GetCardIndex());
        }
        RpcInitPlayer(cards_indexs);
        //foreach (CardGUI card in cards)
        //player_cards.Add(card.card.GetCardIndex());
    }

    public void GetCardFromKupa(CardGUI card)
    {
        card.gameObject.SetActive(true);
        NetworkServer.Spawn(card.gameObject, connectionToClient);
        PushCard(card);
    }

    public void PushCard(CardGUI card)
    {
        playerHand.PushCard(card.card);
        //Debug.Log(handGui);
        //RpcShowCard(card);
        //if (handGui != null)
        //    RpcShowCard(card);
        //else
        //{
        //    Debug.Log("hand null");
        //}
    }


    [ClientRpc]
    public void RpcShowCard(int index)
    {
        Debug.Log("Rpc show card");
        if (handGui == null)
            return;
        //handGui.ShowCard(index);
        //Debug.Log(handGui.HandView);
    }

    [ClientRpc]
    public void RpcInitPlayer(List<int> indexs)
    {
        Debug.Log("Rpc show card");
        if (handGui == null)
            return;
        List<CardGUI> cards = new List<CardGUI>();
        foreach(int i in indexs)
        {
            CardGUI card = MatchController.cards[i];
            if (!hasAuthority)
            {
                Debug.Log("enemy card: " + i);
                card.setFaceDown(true);
            }
            cards.Add(card);
        }
        if (isServer)
            playerHand.PrintDeck();
        handGui.InitHand(cards);
        //Debug.Log(handGui.HandView);
    }

    public void OnClickCard(CardGUI card)
    {
        if (!card.card.isAvailable()) return;
        CardPressed(card);
        Debug.Log("pressed" + card);
    }

    public void CardPressed(CardGUI cardGUI)
    {
        Debug.Log(cardGUI);
        int selected_cards_size = selected_cards.Count;
        Card c = cardGUI.card;
        //if (c.IsSelected())
        //{
        //    c.SetSelected(false);
        //    //Debug.Log("UnSelecting Card");
        //    Hand.Selected_cards.Remove(c);
        //    return;
        //}
        //GameState.GetGameState()
        if (cardGUI.selected)
        {
            cardGUI.SetSelected(false);
            //Debug.Log("UnSelecting Card");
            selected_cards.Remove(cardGUI);
            return;
        }
        Debug.Log(MatchController.GameState);
        Debug.Log("size: " + selected_cards_size);
        if ((MatchController.GameState != MatchController.GameStates.START && selected_cards_size != 0))
        {
            if (selected_cards[0].card.getValue() != c.getValue())
            {
                foreach (CardGUI selected in selected_cards)
                    selected.SetSelected(false);
                selected_cards.Clear();
            }
            cardGUI.SetSelected(true);
            Debug.Log(cardGUI + "pressed on game play");
            selected_cards.Add(cardGUI);
        }
        else if (selected_cards_size < 3)
        {
            Debug.Log("pick open card");
            cardGUI.SetSelected(true);
            selected_cards.Add(cardGUI);
            //Debug.Log("You can only choose 3 cards");
        }
        //if (selected_cards_size >= 0 && selected_cards_size < 3)
        //	if (GameState.GetGameState() == GameState.GameStates.START || Hand.Selected_cards[0].getValue() == c.getValue())
        //          {
        //              c.SetSelected(true);
        //              Hand.Selected_cards.Add(c);
        //          }
    }

    public void PushCard(Card card)
    {
        playerHand.PushCard(card);
        //Debug.Log(handGui);
        //RpcShowCard(card.GetCardIndex());
        //if (handGui != null)
        //    RpcShowCard(card);
        //else
        //{
        //    Debug.Log("hand null");
        //}
    }

    [ClientRpc]
    public void RpcUpdateHand(List<Card> cards)
    {
        Debug.Log("update");
        handGui.UpdateHand(cards);
    }



    [Command]
    public void CmdTest()
    {
        Debug.Log("Command");
    }


    [ClientRpc]
    public void RpcShowCard(CardGUI card)
    {
        Debug.Log("Rpc show card");
        //if(matchController.PlayerHands[card.connectionToClient] == null)
        //{
        //    Debug.Log("null");
        //    return;
        //}
        //matchController.PlayerHands[card.connectionToClient].ShowCard(card);
        if (handGui == null)
            return;
        handGui.ShowCard(card);
        Debug.Log(handGui.HandView);
    }

    void OnDeckUpdated(SyncList<int>.Operation op, int index, int oldCard, int newCard)
    {
        switch (op)
        {
            case SyncList<int>.Operation.OP_ADD:
                //Debug.Log("Card added: " + newCard.ToString());
                // index is where it was added into the list
                // newItem is the new item
                break;
            case SyncList<int>.Operation.OP_INSERT:
                // index is where it was inserted into the list
                // newItem is the new item
                break;
            case SyncList<int>.Operation.OP_REMOVEAT:
                // index is where it was removed from the list
                // oldItem is the item that was removed
                break;
            case SyncList<int>.Operation.OP_SET:
                // index is of the item that was changed
                // oldItem is the previous value for the item at the index
                // newItem is the new value for the item at the index
                break;
            case SyncList<int>.Operation.OP_CLEAR:
                // list got cleared
                break;
        }
    }


    //public void InitPlayer(List<CardGUI> cards)
    //{
    //    playerHand.hand.
    //}    
}
