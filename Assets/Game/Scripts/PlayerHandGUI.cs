using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHandGUI : MonoBehaviour
{
    public GameObject HandView;
    public GameObject HiddenCardsView;
    public GameObject LayoutElementPrefab;
    public Dictionary<CardGUI, GameObject> hand_elements = new Dictionary<CardGUI, GameObject>();
    //public Dictionary<Card, CardGUI> player_cards;

    //[SerializeReference]
    //public Deck hand;

    public void ShowCard(CardGUI cardGUI)
    {
        //hand.ShowCard(cardGUI.card);
        GameObject element = Instantiate(LayoutElementPrefab, HandView.transform);
        cardGUI.transform.SetParent(element.transform, false);
        hand_elements.Add(cardGUI, element);
    }

    public void InitHand(List<CardGUI> cards)
    {
        //hand = new Deck();
        foreach(CardGUI card in cards)
        {
            ShowCard(card);
        }
    }

    //public CardGUI PopCard(CardGUI card)
    //{
    //}

    //public void HideFirstThreeCards()
    //{
    //    for(int i = 0; i < 3; i++)
    //    {
    //        Card card = hand.hand.getCard(i);
    //    }
    //}

    private void HideCard(CardGUI card)
    {
        //player_cards[card].setFace_down(true);
    }
}
