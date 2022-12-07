using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    private void PileInputCheck()
    {

    }

    public void AddCardToPile(CardGUI card)
    {
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
        Debug.Log("clicked" + eventData.pointerCurrentRaycast.gameObject.name);
        match_controller.CmdMakePlay();
    }
}
