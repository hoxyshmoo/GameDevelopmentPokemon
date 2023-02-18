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
    




    public void SetDialog(string text)
    {
        DialogText.text = text;
    }

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

    public void EnableDiaglogText(bool enabled)
    {
        DialogText.enabled = enabled;
    }

    public void EnableSelectAction(bool enabled)
    {
        SelectAction.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        MoveSelector.SetActive(enabled);
        MoveDetails.SetActive(enabled);
    }
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