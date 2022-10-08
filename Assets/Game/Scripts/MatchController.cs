using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchController : NetworkBehaviour
{
    [SerializeReference]
    internal readonly Dictionary<Card, CardGUI> MatchCards = new Dictionary<Card, CardGUI>();

    public List<GameObject> HandViewList;

    public Dictionary<NetworkIdentity, GameObject> HandsView = new Dictionary<NetworkIdentity, GameObject>();
    public Dictionary<NetworkIdentity, MatchPlayerData> players = new Dictionary<NetworkIdentity, MatchPlayerData>();

    public KupaGUI kupaGUI;
    public Deck kupa;
    // Start is called before the first frame update
    void Start()
    {
        kupaGUI.FillMatchCards();
        Debug.Log(MatchCards.Count);
        int index = 0;
        foreach(var i in CanvasController.connections)
        {
            //HandsView.Add(, HandViewList[index]);
            index++;
        }
        //CardGUI cardGUI = kupaGUI.GetCardFromKupa();
        //playerHand.PushCard(cardGUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
