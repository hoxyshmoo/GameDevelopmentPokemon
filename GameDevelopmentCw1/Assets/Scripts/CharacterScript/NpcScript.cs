using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, Interactable
{

[SerializeField] Dialogue dialogue;

//For Testing Animation of NPC
// [SerializeField] List<Sprite> sprites;

// SpriteAnimator spriteAnimate;

// private void Start(){
//     spriteAnimate=new SpriteAnimator(sprites,GetComponent<SpriteRenderer>());
//     spriteAnimate.Start();
// }

// private void Update(){
//     spriteAnimate.HandleUpdate();
// }

    public void Interact(){
        //Debug.Log("Test Interaction");
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue));
    }
}
