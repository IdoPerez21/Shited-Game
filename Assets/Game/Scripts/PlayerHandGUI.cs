using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandGUI : MonoBehaviour
{
    public GameObject HandView;
    public GameObject HiddenCardsView;
    public GameObject LayoutElementPrefab;
    public Dictionary<CardGUI, GameObject> hand_elements = new Dictionary<CardGUI, GameObject>();
    public Dictionary<Card, CardGUI> player_cards;

    public PlayerHand playerHand;

    public void PushCard(CardGUI cardGUI)
    {
        playerHand.hand.PushCard(cardGUI.card);
        GameObject element = Instantiate(LayoutElementPrefab, HandView.transform);
        cardGUI.transform.SetParent(element.transform, false);
        hand_elements.Add(cardGUI, element);
    }

    public CardGUI PopCard(int index)
    {
        Card card = playerHand.hand.PopCard(index);
        return player_cards[card];
    }

    public void HideFirstThreeCards()
    {
        for(int i = 0; i < 3; i++)
        {
            Card card = playerHand.hand.getCard(i);
        }
    }

    private void HideCard(Card card)
    {
        player_cards[card].setFace_down(true);
    }
}
