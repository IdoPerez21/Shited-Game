using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
	public List<Card> cards_list = new();
	[SerializeField]
	private List<Card> selected_cards;
	public List<Card> Selected_cards { get => selected_cards; set => selected_cards = value; }

	//public List<Card> Table_cards { get => table_cards; set => table_cards = value; }

	// Start is called before the first frame update
	public Deck(bool FillDeck)
	{
		selected_cards = new List<Card>();
		FillDeckCards();
    }

	public Deck()
	{
		selected_cards = new List<Card>();
	}

    private void FillDeckCards()
    {
        for (int j = 0; j < 4; j++)
            for (int i = 1; i < 14; i++)
            {
				Card card = new Card(i, j);
				cards_list.Add(card);
            }
        //for (int i = 0; i < 2; i++) // jokers
        //    CreateCard(15, i);
    }

    public List<Card> GetCardsList()
	{
		return cards_list;
	}

	public void ClearSelection()
	{
		selected_cards.Clear();
	}

	public Card Peak()
	{
		return cards_list[cards_list.Count - 1];
	}

	//public Deck GetRandomDeck(int amount)
	//{
	//	Deck d = new Deck();


	//	for (int i = 0; i < amount; i++)
	//	{
	//		int index = Random.Range(0, cards_list.Count);
	//		d.PushCard(PopCard(index));
	//	}
	//	return d;
	//}

	public List<Card> GetRandomDeck(int amount)
	{
		List<Card> d = new();

		Debug.Log(cards_list.Count);
		for (int i = 0; i < amount; i++)
		{
			d.Add(PopRandomCard());
		}
		return d;
	}

	//public List<long> GetCardsIndexs(List<Card> cards)
	//   {
	//	List<long> indexs = new();
	//	foreach (Card card in cards)
	//		indexs.Add(card.GetCardIndex());
	//	return indexs;
	//   }

	public List<Card> GetOpenCards()
	{
		List<Card> open_cards = new List<Card>();
		foreach (Card card in cards_list)
		{
			if (card.isOpen_card())
				open_cards.Add(card);
		}

		return open_cards;
	}

	public virtual Card PopRandomCard()
	{
		int index = Random.Range(0, cards_list.Count);
		return PopCard(index);
	}

	public void AddDeck(Deck d)
	{
		foreach (Card c in d.GetCardsList())
		{
			//if (c.getActionListeners().length > 0)
			//	c.removeActionListener(c.getActionListeners()[0]);
			PushCard(c);
		}

		d.GetCardsList().Clear();
	}

	public Card GetPileTopCard()
	{
		int index = cards_list.Count - 1;
		while (index > 0 && cards_list[index].getValue() == 3)
			index--;
		return cards_list[index];
	}

	public Card getLowestCard()
	{
		long cardValue;
		Card low = new Card(100);
		foreach (Card c in cards_list)
		{
			if (c.isAvailable())
			{
				cardValue = c.getValue();
				if (cardValue == 4)
					return c;
				if (cardValue > 3 && cardValue < low.getValue() && cardValue != 10)
					low = c;
			}
		}
		return low;
	}

	public Card getCard(int index)
	{
		return cards_list[index];
	}

	public int GetDeckSize()
	{
		return cards_list.Count;
	}

	public virtual void PushCard(Card card)
	{
		//card.transform.SetParent(transform);
		cards_list.Add(card);
		//if (dpanel != null)
		//{
		//	dpanel.AddCard(card);
		//	if (card.getActionListeners().length == 0)
		//		AddCardActionListener(card);
		//}
	}

	public void clearDeck()
	{
		cards_list.Clear();
		//if (dpanel != null)
		//	dpanel.removeAll();
	}

	public void RemoveSelectedCards()
	{
		cards_list.RemoveAll(c => selected_cards.Contains(c));
		//if (dpanel != null)
		//	for (Card c : selected_cards)
		//		dpanel.remove(c);
		ClearSelection();
	}

	public virtual Card PopCard(int index)
	{
		Card c = cards_list[index];
		//if (dpanel != null)
		//{
		//	dpanel.remove(c);
		//}
		cards_list.RemoveAt(index);
		return c;
	}

	public virtual Card PopCard(Card c)
	{
		//if (dpanel != null)
		//{
		//	dpanel.remove(c);
		//}
		cards_list.Remove(c);
		return c;
	}

	public void PrintDeck()
	{
		foreach (Card card in cards_list)
			card.Print();
	}

	public string AsString()
	{
		string str = "";
		foreach (Card c in cards_list)
		{
			str += c.getValue() + ", ";
		}
		return str;
	}

	public void setCardsList(List<Card> cards)
	{
		cards_list = cards;
		//foreach(Card card in cards_list)
		//	card.transform.SetParent(transform);
	}

	public bool isEmpty()
	{
		return cards_list.Count == 0;
	}

	//public void SetActive(bool active)
	//{
	//	foreach (Card card in cards_list)
	//	{
	//		card.SetActive(active);
	//	}
	//}
}
