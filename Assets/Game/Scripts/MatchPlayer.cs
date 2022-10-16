using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MatchPlayer : NetworkBehaviour
{
    public PlayerHandGUI handGui;
    public Deck playerHand;
    public int index;
    public MatchController matchController;

    void Update()
    {
        //Debug.Log("Auth: " + connectionToClient);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isClientOnly)
        {
            //Debug.Log(connectionToClient);
            matchController = GameObject.FindObjectOfType<MatchController>();
        }
        Debug.Log(connectionToClient);
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
    public void InitPlayer(List<CardGUI> cards)
    {
        playerHand = new();
        foreach (CardGUI card in cards)
            GetCardFromKupa(card);
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
        TargetShowCard(card);
        //if (handGui != null)
        //    RpcShowCard(card);
        //else
        //{
        //    Debug.Log("hand null");
        //}
    }

    [TargetRpc]
    public void TargetShowCard(CardGUI card)
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


    //public void InitPlayer(List<CardGUI> cards)
    //{
    //    playerHand.hand.
    //}    
}
