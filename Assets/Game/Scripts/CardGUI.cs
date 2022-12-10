using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;
using UnityEngine.Events;

public class CardGUI : NetworkBehaviour
{
	[Header("Card Script", order = 1)]
	[SerializeReference]
	public Card card;

	[Header("GUI elements", order = 2)]
	public Button CardBtn;
    public Sprite Face_up_image, Face_down_image;
	public GameObject CardPrefab;

    [SerializeField]
	private float Hidden_card_size, Normal_card_size, Pile_card_size;

	public bool selected, face_down, avalible;


    void Awake()
    {
        selected = false;
		face_down = false;
		avalible = false;
    }

  //  public override void OnStartClient()
  //  {
  //      base.OnStartClient();
		////GetComponent<Image>().sprite = hasAuthority ? Face_up_image : Face_down_image;
		////Debug.Log
  //  }

	public void CardReset()
    {
		setOpen_card(false);
		setFaceDown(false);
		SetAvalible(true);
		SetPileCard(false);
		if(selected)
			SetSelected(false);
		GetComponent<Button>().onClick.RemoveAllListeners();
    }

	public void AddClickListener(UnityAction onClick)
    {
		GetComponent<Button>().onClick.AddListener(onClick);
    }

    public void SetFaceUpImage(Sprite faceUpSprite)
    {
		Face_up_image = faceUpSprite;
		GetComponent<Image>().sprite = faceUpSprite;
		//GetComponent<Image>().type = Image.Type.Simple;
    }

    // Start is called before the first frame update
	public void setFaceDown(bool face_down)
	{
		this.face_down = face_down;
		avalible = !face_down;
		Image image = GetComponent<Image>();
		image.sprite = face_down ? Face_down_image : Face_up_image;
		GetComponent<Button>().enabled = !face_down;
        //Debug.Log("Card face down : " + image.sprite);
	}

	public void SetPileCard(bool value)
    {
		CardBtn.enabled = !value;
		GetComponent<RectTransform>().sizeDelta = value ? new Vector2(Pile_card_size, Pile_card_size) : new Vector2(Normal_card_size, Normal_card_size);
	}

	public void SetTableCard(bool value)
    {
		CardBtn.enabled = !value;
		//SetInteractable(!value);
		if (value)
		{
			GetComponent<RectTransform>().sizeDelta = new Vector2(Hidden_card_size, Hidden_card_size);
		}
		else
		{
			//SetInteractable(true);
			GetComponent<RectTransform>().sizeDelta = new Vector2(Normal_card_size, Normal_card_size);
		}
	}

	public void setOpen_card(bool open_card)
	{
		//card.SetOpen_card(open_card);
		//this.open_card = open_card;
		setFaceDown(!open_card);
		SetTableCard(open_card);
        if (open_card)
        {
			//setBackground(Color.BLUE);
			//setEnabled(false);
			//available = false;
			//GetComponent<SpriteRenderer>().sortingLayerName = "OpenCard";
			transform.Rotate(-Vector3.forward * 6f);
			transform.localPosition = Vector3.right * 4f;
        }
        else
        {
			//setEnabled(true);
			//GetComponent<SpriteRenderer>().sortingLayerName = "Card";
			//available = true;
			//setBackground(Color.WHITE);
			transform.rotation = new Quaternion(0, 0, 0, 0);
			transform.localPosition = Vector3.zero;
		}
	}


	public void SetInteractable(bool value)
	{
		//Image image = GetComponent<Image>();
		//Debug.Log("enabled : " + enabled + " new value : " + value);
		if (value)
		{
			CardBtn.interactable = true;
			//transform.localScale = Vector3.one;
			//Debug.Log("card enabled");
		}
		else
		{
			CardBtn.interactable = false;
			//transform.localScale -= Vector3.one * 0.1f;
			//Debug.Log("card unenabled");
		}
	}

	public void SetAvalible(bool value)
    {
		avalible = value;
		CardBtn.enabled = value;
		Image image = GetComponent<Image>();
		if(value)
        {
			//Debug.Log("Card avalible");
			image.color = Color.white;
        }
        else
        {
			//Debug.Log("Card unavalible");
			image.color = Color.gray;
        }
    }

	public void SetSelected(bool value)
	{
		selected = value;
		if (value)
		{
			Debug.Log("card selected");
			//transform.position.Set(transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z);
			//transform.up *= 0.5f;
			//GetComponent<SpriteRenderer>().sortingLayerName = "PickedCard";
			transform.localPosition += Vector3.up * 1.5f;
		}
		else
		{
			//GetComponent<SpriteRenderer>().sortingLayerName = "Card";
			transform.localPosition -= Vector3.up * 1.5f;
		}
		//transform.localPosition.Set(transform.localPosition.x, transform.localPosition.y - 1f, transform.localPosition.z);
	}

    public void UpdateUI()
    {
		if (card.isFace_down())
			setFaceDown(true);
		if (!card.isOpen_card())
			setOpen_card(true);

    }

    public bool IsSeleted() { return selected; }

	public void SetActive(bool active) { gameObject.SetActive(active); }
}
