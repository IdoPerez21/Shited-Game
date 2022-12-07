using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class MatchPlayer : NetworkBehaviour
{
    public PlayerHandGUI handGui;
    [SerializeReference]
    public Deck playerHand;
    public readonly SyncList<int> player_cards = new();
    public UnityAction player_ready_action;

    public bool player_ready;
    //public SyncList<int> selected_cards = new();
    public List<int> selected_cards = new();

    public int index;
    public MatchController matchController;

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
        card.CardReset();
        if (!hasAuthority)
            card.setFaceDown(true);
        card.AddClickListener(OnClickCardListener(card));
        handGui.PushCard(card);
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
        //Debug.Log("Rpc show cards: " + connectionToClient);
        if (handGui == null)
            return;
        List<CardGUI> cards = new List<CardGUI>();
        foreach(int i in indexs)
        {
            CardGUI card = MatchController.cards[i];
            if (!hasAuthority)
            {
                //Debug.Log("enemy card: " + i);
                card.setFaceDown(true);
            }
            else
                card.CardBtn.onClick.AddListener(OnClickCardListener(card));
            cards.Add(card);
        }
        //if (isServer)
        //    playerHand.PrintDeck();
        Debug.Log(handGui + " " + cards.Count);
        handGui.InitHand(cards);
        //Debug.Log(handGui.HandView);
    }

    public bool SetRelevantCards(Card check_card)
    {
        Debug.Log("Set relevents cards");
        bool hasRelevent = false, isRelevent;
        int check_value = check_card.getValue();
        List<int> relevents_cards_indexs = new List<int>();
        foreach (Card c in playerHand.cards_list)
        {
            isRelevent = false;
            //c.setEnabled(false);
            if (c.isAvailable())
            {
                //Debug.Log("Card avalible: " + c.getValue());
                if (c.isSpecialCard() || check_value == 2)
                {
                    hasRelevent = true;
                    //c.setEnabled(true);
                    continue;
                }
                int value = c.getValue();
                // Standard rule - if card value is higher or equal//				
                if (check_value != 7)
                {
                    hasRelevent |= value >= check_value;
                    isRelevent = value >= check_value;
                    //c.setEnabled(value >= check_value);
                }
                else
                { // if value is 7, check if value is lower or equal to 7
                    hasRelevent |= value <= check_value;
                    isRelevent = value <= check_value;

                    //c.setEnabled(value <= check_value);
                }
                if (!isRelevent)
                {
                    //Debug.Log("Card relevent: " + c.getValue());
                    relevents_cards_indexs.Add(c.GetCardIndex());
                }
            }



        }
        TargetSetCardsRelevent(connectionToClient, relevents_cards_indexs);
        return hasRelevent;
    }

    public Deck getHand()
    {
        return playerHand;
    }

    [TargetRpc]
    public void TargetSetCardsRelevent(NetworkConnection conn, List<int> indexs)
    {
        //Debug.Log(indexs.Count + "indexs");
        handGui.SetReleventsCards(MatchController.GetCardsListByIndexs(indexs));
    }

    public void SetSelectedAsOpenCards()
    {
        foreach(int i in selected_cards)
        {
            CardGUI card = MatchController.GetCardByIndex(i);
            card.card.SetOpen_card(true);
        }
        RpcShowOpenCards(selected_cards);
        selected_cards.Clear();
    }

    [ClientRpc]
    public void RpcShowOpenCards(List<int> selected_cards)
    {
        Debug.Log("Show open cards");
        //Debug.Log("selected cards size: " + selected_cards.Count);
        List<CardGUI> cards = new();
        foreach (int i in selected_cards)
        {
            CardGUI card = MatchController.GetCardByIndex(i);
            card.CardReset();
            card.setOpen_card(true);
            cards.Add(card);
        }
        handGui.SetOpenCards(cards);
    }

    //public void OnClickCardListener(CardGUI card)
    //{
    //    if (!card.card.isAvailable()) return;
    //    CardPressed(card);
    //    Debug.Log("pressed" + card);
    //}

    public UnityAction OnClickCardListener(CardGUI card)
    {
        return new (() => {
            CmdCardPressed(card.card.GetCardIndex());
            Debug.Log("pressed" + card);
        });
        //CardPressed(card);
    }

    [Command]
    public void CmdCardPressed(int index)
    {
        CardGUI cardGUI = MatchController.GetCardByIndex(index);
        if (!cardGUI.card.isAvailable()) return;

        //Debug.Log(cardGUI);
        int selected_cards_size = selected_cards.Count;
        Card c = cardGUI.card;

        Debug.Log(MatchController.GameState);
        Debug.Log("size: " + selected_cards.Count);

        if (cardGUI.selected)
        {
            //cardGUI.SetSelected(false);
            TargetSetCardSelected(connectionToClient, c.GetCardIndex(), false);
            //Debug.Log("UnSelecting Card");
            selected_cards.Remove(cardGUI.card.GetCardIndex());
            return;
        }

        if (selected_cards.Count == 0) 
        {
            selected_cards.Add(cardGUI.card.GetCardIndex());
            TargetSetCardSelected(connectionToClient, c.GetCardIndex(), true);
            return;
        }

        if ((MatchController.GameState != MatchController.GameStates.START && selected_cards_size <= 4))
        {
            CardGUI selectedCard = MatchController.GetCardByIndex(selected_cards[0]);
            if (selectedCard.card.getValue() != c.getValue())
            {
                foreach (int selected in selected_cards)
                    TargetSetCardSelected(connectionToClient, selected, false);

                //MatchController.GetCardByIndex(selected).SetSelected(false);
                selected_cards.Clear();
            }
            //cardGUI.SetSelected(true);
            selected_cards.Add(cardGUI.card.GetCardIndex());
            TargetSetCardSelected(connectionToClient, c.GetCardIndex(), true);
        }
        else if (selected_cards_size < 3)
        {
            Debug.Log("pick open card");
            //cardGUI.SetSelected(true);
            selected_cards.Add(cardGUI.card.GetCardIndex());
            TargetSetCardSelected(connectionToClient, c.GetCardIndex(), true);
            //Debug.Log("You can only choose 3 cards");
        }
        
    }
    [TargetRpc]
    public void TargetSetCardSelected(NetworkConnection target, int index, bool selected)
    {
        Debug.Log(index + "got selected");
        MatchController.GetCardByIndex(index).SetSelected(selected);
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
        //handGui.ShowCard(card);
        Debug.Log(handGui.HandView);
    }
}
