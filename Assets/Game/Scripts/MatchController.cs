using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchController : MonoBehaviour
{
    [SerializeReference]
    internal readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public Dictionary<NetworkIdentity, GameObject> HandsView = new Dictionary<NetworkIdentity, GameObject>();

    public KupaGUI kupaGUI;
    public Deck kupa;
    // Start is called before the first frame update
    void Start()
    {
        kupaGUI.FillMatchCards();
        Debug.Log(MatchCards.Count);
        //CardGUI cardGUI = kupaGUI.GetCardFromKupa();
        //playerHand.PushCard(cardGUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
