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
    public void UpdateHand(List<Card> cards)
    {
        Debug.Log("Update hand");
        foreach (Card card in cards)
        {
            card.Print();
            ShowCard(MatchController.MatchCards[card]);
        }
    }
    public void ShowCard(CardGUI cardGUI, Transform view = null)
    {
        if (view == null)
            view = HandView.transform;
        //hand.ShowCard(cardGUI.card);
        //Debug.Log("show: " + index);
        //CardGUI cardGUI = MatchController.cards[index];
        //if (!GetComponent<NetworkIdentity>().hasAuthority)
        //    cardGUI.setFaceDown(true);
        //Debug.Log(cardGUI);
        GameObject element = Instantiate(LayoutElementPrefab, view);
        cardGUI.transform.SetParent(element.transform, false);
        hand_elements.Add(cardGUI, element);
        cardGUI.gameObject.SetActive(true);
    }

    public void InitHand(List<CardGUI> cards)
    {
        //hand = new Deck();
        //foreach (CardGUI card  in cards)
        //    Debug.Log(card);
        int i = 0;
        foreach (CardGUI card in cards)
        {
            if (i < 3)
            {
                //Debug.Log("hide card index: " + cards[i]);
                HideCard(card);
                i++;
            }
            else
            {
                //Debug.Log("show card index: " + cards[i]);
                ShowCard(card);
            }
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
        card.setFaceDown(true);
        ShowCard(card, HiddenCardsView.transform);
        //player_cards[card].setFaceDown(true);
    }
}
