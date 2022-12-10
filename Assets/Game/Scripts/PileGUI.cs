using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PileGUI : MonoBehaviour, IPointerClickHandler
{
    public List<CardGUI> pile_cards = new();
    public float Rotation = 10f;
    public float start_rotation = 0;
    private float card_rotation;
    public MatchController match_controller;


    void Awake()
    {
       match_controller = GetComponentInParent<MatchController>();
    }

    public void AddCardToPile(CardGUI card)
    {
        card.CardReset();
        Debug.Log("Added card to pile");
        //GetComponent<Image>().sprite = card.Face_up_image;
        if (pile_cards.Count == 0)
            card_rotation = start_rotation;
        else
            card_rotation = Rotation;
        
        card.transform.SetParent(transform,false);
        card.SetPileCard(true);
        card.transform.Rotate(0, 0, card_rotation);
        Rotation *= -1;
        pile_cards.Add(card);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("clicked" + eventData.pointerCurrentRaycast.gameObject.name);
        match_controller.MakePlay();
    }

    public void ClearPile()
    {
        Debug.Log("pile cards amount:" + pile_cards.Count);
        foreach (CardGUI card in pile_cards)
            Destroy(card.gameObject);
        pile_cards.Clear();
    }
}
