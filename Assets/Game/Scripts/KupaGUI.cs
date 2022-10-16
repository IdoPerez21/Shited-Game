using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KupaGUI : MonoBehaviour
{
    public Deck kupa = new(FillDeck: true);

    [Header("Assigned in inspector")]
    public List<Sprite> FaceUp_Sprites;
    public GameObject CardPrefab;
    //public MatchController MatchController;
    public Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillMatchCards()
    {
        int index = 0;
        foreach(Card card in kupa.cards_list)
        {
           MatchCards.Add(card, CreateCardGUI(card, index));
            index++;
        }
    }
    public CardGUI GetCardFromKupa()
    {
        return MatchCards[kupa.PopRandomCard()];
    }

    public List<CardGUI> GetRandomDeck(int amount)
    {
        List<Card> deck = kupa.GetRandomDeck(amount);
        List<CardGUI> cards = new();
        foreach (Card card in deck) cards.Add(MatchCards[card]);
        return cards;
    }

    private CardGUI CreateCardGUI(Card card, int index)
    {
        CardGUI cardGui = Instantiate(CardPrefab, transform).GetComponent<CardGUI>();
        cardGui.card = card;
        cardGui.SetFaceUpImage(FaceUp_Sprites[index]);
        cardGui.CardPrefab = CardPrefab;
        cardGui.name = "CardValue" + card.getValue() + "Shape" + card.GetShape();
        cardGui.SetActive(false);
        return cardGui;
    }
}
