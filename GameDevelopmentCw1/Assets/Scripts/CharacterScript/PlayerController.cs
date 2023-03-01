using System;    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
    // variables for the player controller
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;

    //public float moveSpeed;
    //private Animator animator; //Original Unity Animator
    //private CharacterAnimator animator; //New Unity Animator
    private Character character;

    //private bool isMoving; //depreceted cause of unused
    private Vector2 input;
    // when battle is started
    //public event Action OnEncountered;
    public event Action<Collider2D> OnEnterTrainerView;
    
    private void Awake(){
       // animator=GetComponent<Animator>(); //Original Unity Animator
      // animator=GetComponent<CharacterAnimator>();  //New Unity Animator
       character=GetComponent<Character>();
    }

    //get user input and assign it to variables
    public void HandleUpdate(){
        if(!character.IsMoving){
input.x=Input.GetAxisRaw("Horizontal");
    input.y=Input.GetAxisRaw("Vertical");

    //remove diagonal movement
        if(input.x!=0) {
            input.y=0;
            }

    if(input!=Vector2.zero){
//         // animator.SetFloat("moveX",input.x); //Original Unity Animator
//         // animator.SetFloat("moveY",input.y);

//         animator.MoveX=input.x; //New Unity Animator
//         animator.MoveY=input.y;

//         var TargetPosition=transform.position;
//         TargetPosition.x+=input.x;
//         TargetPosition.y+=input.y;

//         if(isWalkable(TargetPosition)){
// StartCoroutine(Move(TargetPosition));
//         }
StartCoroutine(character.Move(input,OnMoveOver));
        
    }
        }

        character.HandleUpdate();

    // animator.SetBool("isMoving", isMoving); //Original Unity Animator
    //animator.isMoving=isMoving;  //uneeded

    if(Input.GetKeyDown(KeyCode.Z)){
        Interact();
    }
    }

    //Interact function
    void Interact(){
        //var DirectionFacing = new Vector3(animator.GetFloat("moveX"),animator.GetFloat("moveY")); Original Unity Animator
        var DirectionFacing = new Vector3(character.Animator.MoveX,character.Animator.MoveY); //New Unity Animator
        var interacPosition=transform.position+DirectionFacing;

        //Debug.DrawLine(transform.position,interacPosition,Color.green,0.5f); //Test Direction Facing

       var collider= Physics2D.OverlapCircle(interacPosition,0.3f,GameLayers.i.InteractableLayer); //check if the collider is touching a game interactable layer object
       if(collider!=null){ //check if collider is null or interactable
        collider.GetComponent<Interactable>()?.Interact(transform);
       }
    }

    //check if trainer in view using collision circle
    private void CheckIfInTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.FovLayer); //check if trainer is in fov layer


        if (collider != null )
        {
            character.Animator.isMoving = false;
            OnEnterTrainerView?.Invoke(collider);
        }
    }

//Commented due to importing to Character.cs for more reusable code
    // IEnumerator Move(Vector3 TargetPosition){

    // isMoving=true;

    //     while((TargetPosition-transform.position).sqrMagnitude> Mathf.Epsilon){
    //         transform.position=Vector3.MoveTowards(transform.position,TargetPosition,moveSpeed*Time.deltaTime);
    //         yield return null;
        
    //     }
    //     transform.position=TargetPosition;
    //     isMoving=false;

    //     CheckForEncounters();
    // }

    // private bool isWalkable(Vector3 TargetPosition){
    //  if(Physics2D.OverlapCircle(TargetPosition,0.2f,interactableLayer | SolidObjectsLayer)!=null){  
    //     return false;
    //  }
    
    //  return true;
    // }


    private void OnMoveOver(){
        var colliders = Physics2D.OverlapCircleAll(transform.position-new Vector3(0,character.OffsetY),0.2f,GameLayers.i.TriggerableLayer);         
  
        foreach (var collide in colliders)
        {
            var triggerable= collide.GetComponent<PlayerTriggerable>();
            if(triggerable!=null){
                 character.Animator.isMoving=false; // New Unity Animation
                triggerable.OnplayerTriggered(this);    
                break;
            }
        }
        CheckIfInTrainersView();
    }

//expose variables for easy access
    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    //Removed to put function into is its own file for reuseability
    // private void CheckForEncounters(){

    //     if(Physics2D.OverlapCircle(transform.position,0.2f,GameLayers.i.GrassLayer)!=null){
    //         if(UnityEngine.Random.Range(1,101)<=10){
    //             //Debug.Log("Show Encounter");
    //             //animator.SetBool("isMoving", false); //Original Unity Animation
    //             character.Animator.isMoving=false; // New Unity Animation
    //             OnEncountered();
    //         }
    //     }
    // }

    // private void CheckIfInTrainerView(){
    //     var collider = Physics2D.OverlapCircle(transform.position-new Vector3(0,offsetY),0.2f,GameLayers.i)
    // }



    public Character Character =>character;

    }
