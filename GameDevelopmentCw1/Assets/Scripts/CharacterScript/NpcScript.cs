using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour, Interactable
{

//Serialized field to be set by programmer 
[SerializeField] Dialogue dialogue; //takes in dialogue actions
[SerializeField] List<Vector2> movePattern; //maps out npc movement 
[SerializeField] float timeBetweenMovePattern; //maps the time between movements

    public List<Vector2> MovePattern { get { return movePattern; } } //expose move pattern to be accesed by other scripts


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
    //get character component
    character=GetComponent<Character>();
}

    //interaction feature for interacting with objects in the interactable layer 
    public void Interact(Transform initiator){
        //Debug.Log("Test Interaction");
        //check if npc state is in idle form before interacting
        if(state==NPCState.Idle){
            state=NPCState.Dialogue;
            character.LookAttention(initiator.position); //make the npc look at the initiator when trying to start a dialogue
   StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue,()=>{
    idleTimer=0f;
    state=NPCState.Idle; //set state of npc to idle
   }));
        }
     
       // StartCoroutine(character.Move(new Vector2(2,0)));
    }

    private void Update(){
        //Removed as function became redundant
        // if(DialogueManager.Instance.isCurrentlyInDialogue){
        //     return;
        // }

        //start npc movement is npc is in idle state and there is a given move pattern
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

    //Walk function that iterates through the movement patterns and repeats when given pattern has finished 
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

//enum to track state of npc 
public enum NPCState{Idle,Walking,Dialogue}
