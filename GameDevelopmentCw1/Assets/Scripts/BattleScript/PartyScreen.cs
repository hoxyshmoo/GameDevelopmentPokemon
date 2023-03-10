using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Displays party screeen for player
public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;

    // Initilize party member
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
        //Debug.Log("Party Member: " + memberSlots[0]);
        //Debug.Log("Party Member: " + memberSlots[1]);
        //Debug.Log("Party Member: " + memberSlots[2]);
    }

    // Set Data for party members
    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
                memberSlots[i].SetData(pokemons[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Pokemon.";
    }
    // Selects the pokemon on party screen
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }
    // Displays messages in party screen
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}
