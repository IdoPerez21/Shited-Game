using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MatchPlayer : NetworkBehaviour
{
    //PlayerHandGUI playerHand;
    public Deck playerHand;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitPlayer(List<Card> cards)
    {
        playerHand = new Deck();
        playerHand.setCardsList(cards);
    }

    //public void InitPlayer(List<CardGUI> cards)
    //{
    //    playerHand.hand.
    //}    
}
