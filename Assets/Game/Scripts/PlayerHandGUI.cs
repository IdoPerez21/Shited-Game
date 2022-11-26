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
    public Dictionary<CardGUI, GameObject> hidden_cards_elements = new Dictionary<CardGUI, GameObject>();

    //public Dictionary<Card, CardGUI> player_cards;

    //[SerializeReference]
    //public Deck hand;
    public void UpdateHand()
    {

    }

    public void SetOpenCards(List<CardGUI> cards)
    {
        Debug.Log("sets open cards");
        int i = 0;
        foreach (CardGUI card in hidden_cards_elements.Keys)
        {
            Debug.Log("card face down");
            cards[i].transform.SetParent(hidden_cards_elements[card].transform, false);
            Destroy(hand_elements[cards[i]]);
            i++;
            if (i == 3)
                return;
        }
    }

    public void ShowCard(CardGUI cardGUI, Dictionary<CardGUI, GameObject> hand, Transform view = null)
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
        hand.Add(cardGUI, element);
        cardGUI.gameObject.SetActive(true);
    }

    public void PushCard(CardGUI card)
    {
        ShowCard(card, hand_elements);
    }

    public void InitHand(List<CardGUI> cards)
    {
        //hand = new Deck();
        //foreach (CardGUI card  in cards)
        Debug.Log("init hand view");
        int i = 0;
        foreach (CardGUI card in cards)
        {
            //ShowCard(card);
            if (i < 3)
            {
                Debug.Log("hide card index: " + cards[i]);
                HideCard(card);
                i++;
            }
            else
            {
                Debug.Log("show card index: " + cards[i]);
                ShowCard(card, hand_elements);
            }
        }
    }

    public void InitHand()
    {
        Debug.Log("init hand view");
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
        ShowCard(card, hidden_cards_elements, HiddenCardsView.transform);
        //player_cards[card].setFaceDown(true);
    }

    public void RemoveCard(CardGUI card)
    {
        Destroy(hand_elements[card]);
    }
}
