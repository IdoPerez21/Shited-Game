using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KupaGUI : MonoBehaviour
{
    public Deck kupa = new(FillDeck: true);

    [Header("Assigned in inspector")]
    public List<Sprite> FaceUp_Sprites;
    public GameObject CardPrefab;
    public MatchController MatchController;

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
        foreach(Card card in kupa.cards_list)
        {
            MatchController.MatchCards.Add(card, CreateCardGUI(card));
        }
    }
    public CardGUI GetCardFromKupa()
    {
        return CreateCardGUI(kupa.PopRandomCard());
    }

    public List<CardGUI> GetRandomDeck(int amount)
    {
        List<Card> deck = kupa.GetRandomDeck(amount);
        List<CardGUI> cards = new();
        foreach (Card card in deck) cards.Add(CreateCardGUI(card));
        return cards;
    }

    private CardGUI CreateCardGUI(Card card)
    {
        CardGUI cardGui = Instantiate(CardPrefab, transform).GetComponent<CardGUI>();
        cardGui.card = card;
        cardGui.SetFaceUpImage(FaceUp_Sprites[cardGui.card.GetCardIndex()]);
        cardGui.CardPrefab = CardPrefab;
        cardGui.name = "CardValue" + card.getValue() + "Shape" + card.GetShape();
        cardGui.SetActive(false);
        return cardGui;
    }
}
