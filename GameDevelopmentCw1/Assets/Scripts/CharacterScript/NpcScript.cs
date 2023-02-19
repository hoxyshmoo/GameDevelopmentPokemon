using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, Interactable
{

[SerializeField] Dialogue dialogue;

    public void Interact(){
        //Debug.Log("Test Interaction");
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
}
