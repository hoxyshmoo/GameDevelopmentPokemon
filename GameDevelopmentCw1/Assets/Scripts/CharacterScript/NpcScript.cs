using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, Interactable
{

[SerializeField] Dialogue dialogue;
[SerializeField] List<Vector2> movePattern;
[SerializeField] float timeBetweenMovePattern;

    public List<Vector2> MovePattern { get { return movePattern; } }


NPCState state;
float idleTimer;
int currentMovePattern=0;

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

Character character;

private void Awake(){
    character=GetComponent<Character>();
}

    public void Interact(Transform initiator){
        //Debug.Log("Test Interaction");
        if(state==NPCState.Idle){
            state=NPCState.Dialogue;
            character.LookAttention(initiator.position);
   StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue,()=>{
    idleTimer=0f;
    state=NPCState.Idle;
   }));
        }
     
       // StartCoroutine(character.Move(new Vector2(2,0)));
    }

    private void Update(){
        //Removed as function became redundant
        // if(DialogueManager.Instance.isCurrentlyInDialogue){
        //     return;
        // }

        if(state==NPCState.Idle){
            idleTimer+=Time.deltaTime;
            if(idleTimer>timeBetweenMovePattern){
                idleTimer=0f;
                if(movePattern.Count>0){
                StartCoroutine(Walk());
                }
          
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk(){
        state=NPCState.Walking;
        var oldPosition = transform.position;

       yield return character.Move(movePattern[currentMovePattern]);

       if(transform.position!=oldPosition){
       currentMovePattern=(currentMovePattern+1) % movePattern.Count;
       }


        state=NPCState.Idle;
    }
}

public enum NPCState{Idle,Walking,Dialogue}
