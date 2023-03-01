using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{

    //Character animator variable
    CharacterAnimator animator;
    public float moveSpeed; //Movespeed variable
    public bool IsMoving{get; private set;}
    public float OffsetY{get; private set;}=0.3f; //Gives character depth perspective

    private void Awake(){
        animator=GetComponent<CharacterAnimator>(); //Get  character animation component
        SetPositionToProperTile(transform.position); // Set the depth perspective of the main character

    }

    public void SetPositionToProperTile(Vector2 pos){
        //Sets the position of the character to the proper tile center
        pos.x=Mathf.Floor(pos.x)+0.5f;
        pos.y=Mathf.Floor(pos.y)+0.5f+OffsetY;

        transform.position=pos;
    }

    public IEnumerator Move(Vector3 vectorMovement,Action OnMoveOver=null){

        animator.MoveX=Mathf.Clamp(vectorMovement.x,-1f,1f); //New Unity Animator
        animator.MoveY=Mathf.Clamp(vectorMovement.y,-1f,1f);

        var TargetPosition=transform.position; //get current location
        TargetPosition.x+=vectorMovement.x; //update x and y coordinates for target position
        TargetPosition.y+=vectorMovement.y;

        if(!IsPathClear(TargetPosition)){ //continously check if path is clear, if not continue checking, if clear will yield break
            yield break;
        }

    IsMoving=true;

        while((TargetPosition-transform.position).sqrMagnitude> Mathf.Epsilon){
            transform.position=Vector3.MoveTowards(transform.position,TargetPosition,moveSpeed*Time.deltaTime); //update actual character movement
            yield return null;
        
        }
        transform.position=TargetPosition; //assign current position to final position
        IsMoving=false;

        OnMoveOver?.Invoke(); 
        //CheckForEncounters();
    }

    public void HandleUpdate(){
        animator.isMoving=IsMoving; //check if animator is current moving
    }

    private bool IsPathClear(Vector3 TargetPosition){ //path is clear function
        var diffDistance=TargetPosition-transform.position;
        var direction=diffDistance.normalized;


        //physics box to check for collision in path
        if(Physics2D.BoxCast(transform.position+direction,new Vector2(0.2f,0.2f),0f,direction,diffDistance.magnitude-1,GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer)==true){
            return false;
        }
        return true;


    }

    //checks if path shown is walkable
 private bool isWalkable(Vector3 TargetPosition){
    //looks at only solid layer (where solid objects are situated) and the iteractable layer(where interactable objects are situated)
     if(Physics2D.OverlapCircle(TargetPosition,0.2f,GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer)!=null){  
        return false;
     }
    
     return true;
    }

    //Makes the npc look at the main character when interaction with them using the key "z"
    public void LookAttention(Vector3 TargetPosition){
        var xDiff=Mathf.Floor(TargetPosition.x)-Mathf.Floor(transform.position.x);
        var yDiff=Mathf.Floor(TargetPosition.y)-Mathf.Floor(transform.position.y);

        if(xDiff==0 || yDiff==0){
            animator.MoveX=Mathf.Clamp(xDiff,-1f,1f); //New Unity Animator
            animator.MoveY=Mathf.Clamp(yDiff,-1f,1f);
        }
        else{
            Debug.LogError("Error, Cannot make NPC look in diagonal fashion");
        }

    }

    //Expose animator as property
    public CharacterAnimator Animator{
        get => animator;
    }

}
