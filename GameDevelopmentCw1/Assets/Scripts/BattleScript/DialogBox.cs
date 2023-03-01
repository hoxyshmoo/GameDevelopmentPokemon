using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    //Encounter Dialog
    [SerializeField] Text DialogText;
    [SerializeField] int letterSpeed;
    //Select Action
    [SerializeField] GameObject SelectAction;
    [SerializeField] List<Text> actionText;
    [SerializeField] Color highlight;
    //Select Move
    [SerializeField] GameObject MoveSelector;
    [SerializeField] List<Text> moveText;
    //Move Details
    [SerializeField] GameObject MoveDetails;
    [SerializeField] Text PPtext;
    [SerializeField] Text MoveType;
    
    //Sets Dialog in Battle Scenes
    public void SetDialog(string text)
    {
        DialogText.text = text;
    }
    // Animates the text in the dialog
    public IEnumerator TypeDialog(string text)
    {
        DialogText.text = "";
        foreach(var letter in text.ToCharArray())
        {
            DialogText.text += letter;
            yield return new WaitForSeconds(1f / letterSpeed);
        }
        yield return new WaitForSeconds(1f);
    }
    // Shows Dialog text in  battle scene
    public void EnableDiaglogText(bool enabled)
    {
        DialogText.enabled = enabled;
    }
    // Action Menu in battle scene
    public void EnableSelectAction(bool enabled)
    {
        SelectAction.SetActive(enabled);
    }
    // Move Selector for the pokemon moves
    public void EnableMoveSelector(bool enabled)
    {
        MoveSelector.SetActive(enabled);
        MoveDetails.SetActive(enabled);
    }
    //highlight the actions selected
    public void UpdateActionSelection(int select)
    {
        for(int i=0; i<actionText.Count; i++)
        {
            if (i == select)
            {
                actionText[i].color = highlight;
            }
            else
                actionText[i].color = Color.black;
        }
    }
    // Sets the moves name in the move selector
    public void SetMoveNames(List<Moves> moves)
    {
        for(int i=0; i< moveText.Count; i++)
        {
            if (i < moves.Count)
                moveText[i].text = moves[i].Base.name;
            else
                moveText[i].text = "-";
        }
    }
    // Higighlited selected move of the pokemon
    public void UpdateMoveSelection(int select, Moves move)
    { 
        for (int i=0; i<moveText.Count; i++ )
        {
            if (i == select)
                moveText[i].color = highlight;
            else
                moveText[i].color = Color.black;
        }
        PPtext.text = $"PP {move.PP}/{move.Base.PP}";
        MoveType.text = move.Base.Type.ToString();
    }
}