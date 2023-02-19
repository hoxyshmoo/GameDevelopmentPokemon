using System;    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
     public LayerMask SolidObjectsLayer; 
     public LayerMask grassEncounterLayer; 
     public LayerMask interactableLayer; 

    public float moveSpeed;
    private Animator animator;
    private bool isMoving;
    private Vector2 input;
    // when battle is started
    public event Action OnEncountered;
    
    private void Awake(){
        animator=GetComponent<Animator>(); 
    }

    public void HandelUpdate(){
        if(!isMoving){
input.x=Input.GetAxisRaw("Horizontal");
    input.y=Input.GetAxisRaw("Vertical");

    //remove diagonal movement
        if(input.x!=0) {
            input.y=0;
            }

    if(input!=Vector2.zero){
        animator.SetFloat("moveX",input.x);
          animator.SetFloat("moveY",input.y);
        var TargetPosition=transform.position;
        TargetPosition.x+=input.x;
        TargetPosition.y+=input.y;

        if(isWalkable(TargetPosition)){
StartCoroutine(Move(TargetPosition));
        }
        
    }
        }
    animator.SetBool("isMoving", isMoving);

    if(Input.GetKeyDown(KeyCode.Z)){
        Interact();
    }
    }

    
    void Interact(){
        var DirectionFacing = new Vector3(animator.GetFloat("moveX"),animator.GetFloat("moveY"));
        var interacPosition=transform.position+DirectionFacing;

        //Debug.DrawLine(transform.position,interacPosition,Color.green,0.5f); //Test Direction Facing

       var collider= Physics2D.OverlapCircle(interacPosition,0.3f,interactableLayer);
       if(collider!=null){ //check if collider is null or interactable
        collider.GetComponent<Interactable>()?.Interact();
       }
    }

    IEnumerator Move(Vector3 TargetPosition){

    isMoving=true;

        while((TargetPosition-transform.position).sqrMagnitude> Mathf.Epsilon){
            transform.position=Vector3.MoveTowards(transform.position,TargetPosition,moveSpeed*Time.deltaTime);
            yield return null;
        
        }
        transform.position=TargetPosition;
        isMoving=false;

        CheckForEncounters();
    }

    private bool isWalkable(Vector3 TargetPosition){
     if(Physics2D.OverlapCircle(TargetPosition,0.2f,interactableLayer | SolidObjectsLayer)!=null){  
        return false;
     }
    
     return true;
    }

    private void CheckForEncounters(){
        
        if(Physics2D.OverlapCircle(transform.position,0.2f,grassEncounterLayer)!=null){
            if(UnityEngine.Random.Range(1,101)<=10){
                //Debug.Log("Show Encounter");
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }





    }
