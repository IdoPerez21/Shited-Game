using UnityEditor;
using UnityEngine;

public class Card
{
    [SerializeField]
    private int value, shape;

    [SerializeField]
    private bool available, face_down, open_card;

	public Card(int Value, int shape)
	{
		SetValue(Value);
		SetFace_down(false);
		SetShape(shape);
		//available = true;
	}

	public Card(int Value)
	{
		this.value = Value;
	}

	public int getValue() { return value; }
	public void SetValue(int Value)
	{
		if (Value == 1)
		{
			value = 14;
			return;
		}
		value = Value;
	}

	public void SetShape(int shape)
	{
		this.shape = shape;
	}

	public int GetShape() { return shape; }

	public int GetCardIndex() { return value * shape; }

	public bool isFace_down()
	{
		return face_down;
	}

	public void SetFace_down(bool face_down)
	{
		this.face_down = face_down;
		available = !face_down;
	}

	public void SetOpen_card(bool open_card)
	{
		this.open_card = open_card;
		available = !open_card;
	}

	public bool isOpen_card()
	{
		return open_card;
	}


	public bool isSpecialCard()
	{
		return (value == 2 || value == 3 || value == 10);
	}

	public bool isAvailable()
	{
		//		return !(isFace_down() || isOpen_card());
		return available;
	}

	public void setAvailable(bool available)
	{
		this.available = available;
	}

	public void Print()
	{
		Debug.Log("Card: " + getValue() + " faceDown: " + isFace_down()
		+ " available:" + isAvailable() + " opencard: " + isOpen_card());
	}
}
