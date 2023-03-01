using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
//get dialogue components
[SerializeField] GameObject dialogueBox;
[SerializeField] Text dialogueText;
[SerializeField] int letterSpeed;

//create event onshow and on close dialogue
public event Action onShowDialogue;
public event Action onCloseDialogue;

//singleton implementation for dialogue manager instance to allow for access to a singular dialogue object
public static DialogueManager Instance {get; private set;}

private void Awake(){
    //set instance to this dialogue object
    Instance=this;
}

Dialogue dialogue;
Action onDialogFinished;
int currentLine=0;
bool isTyping;

public bool isCurrentlyInDialogue {get; private set;}

//shows npc dialogue 
public IEnumerator ShowDialogue(Dialogue dialogue,Action onFinished=null){
    yield return new WaitForEndOfFrame();
    //if on show dialogue = true, we envoke the event
    onShowDialogue?.Invoke();
    isCurrentlyInDialogue=true;
    this.dialogue=dialogue;
    onDialogFinished=onFinished;

    //show the dialogue box
    dialogueBox.SetActive(true);
    //intialize the dialogue in a typing manner by giving the 0th index of the dialogues lines
    StartCoroutine(TypeDialog(dialogue.Lines[0]));
}

public void HandleUpdate(){
    //handles skips dialogue
if(Input.GetKeyDown(KeyCode.Z) && !isTyping){
++currentLine;
if(currentLine<dialogue.Lines.Count){
    //continue dialogue if current line is less than the dialogue count
    StartCoroutine(TypeDialog(dialogue.Lines[currentLine]));
}
else{
    //else if dialogue is over, set current line back to zero, and disable dialogue box and invoke dialog is finished and close event.
    currentLine=0;
    isCurrentlyInDialogue=false;
    dialogueBox.SetActive(false);
    onDialogFinished?.Invoke();
    onCloseDialogue?.Invoke();
}
}
}

    //outputs the dialogue character by character for more seamless reading
    public IEnumerator TypeDialog(string text)
    {
        isTyping=true;
        dialogueText.text = "";
        foreach(var letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / letterSpeed);
        }   
        isTyping=false;
    } 
}
