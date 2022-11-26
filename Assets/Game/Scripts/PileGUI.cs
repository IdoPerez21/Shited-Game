using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileGUI : MonoBehaviour
{
    public List<CardGUI> pile_cards = new();
    public float Rotation = 10f;
    public float start_rotation = 0;
    private float card_rotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCardToPile(CardGUI card)
    {
        Debug.Log("Add card to pile");
        if (pile_cards.Count == 0)
            card_rotation = start_rotation;
        else
            card_rotation = Rotation;

        card.transform.SetParent(transform,false);
        card.transform.Rotate(0, 0, card_rotation);
        Rotation *= -1;
        pile_cards.Add(card);
    }
}
