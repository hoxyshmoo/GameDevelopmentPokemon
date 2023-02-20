using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
[SerializeField] GameObject dialogueBox;
[SerializeField] Text dialogueText;
[SerializeField] int letterSpeed;

public event Action onShowDialogue;
public event Action onCloseDialogue;

public static DialogueManager Instance {get; private set;}

private void Awake(){
    Instance=this;
}

Dialogue dialogue;
Action onDialogFinished;
int currentLine=0;
bool isTyping;

public bool isCurrentlyInDialogue {get; private set;}

public IEnumerator ShowDialogue(Dialogue dialogue,Action onFinished=null){
    yield return new WaitForEndOfFrame();
    onShowDialogue?.Invoke();
    isCurrentlyInDialogue=true;
    this.dialogue=dialogue;
    onDialogFinished=onFinished;

    dialogueBox.SetActive(true);
    StartCoroutine(TypeDialog(dialogue.Lines[0]));
}

public void HandleUpdate(){
if(Input.GetKeyDown(KeyCode.Z) && !isTyping){
++currentLine;
if(currentLine<dialogue.Lines.Count){
    StartCoroutine(TypeDialog(dialogue.Lines[currentLine]));
}
else{
    currentLine=0;
    isCurrentlyInDialogue=false;
    dialogueBox.SetActive(false);
    onDialogFinished?.Invoke();
    onCloseDialogue?.Invoke();
}
}
}

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
