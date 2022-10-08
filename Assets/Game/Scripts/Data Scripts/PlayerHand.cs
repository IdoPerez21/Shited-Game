using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand
{
    public Deck hand;
    private bool ready, active;
    private List<Card> Selected_cards;

    public PlayerHand(List<Card> hand_deck)
    {
        hand = new()
        {
            cards_list = hand_deck
        };
        ready = false;
        active = true;
    }

    public void HideCard(int index)
    {
        hand.getCard(index).SetFace_down(true);
    }
}
