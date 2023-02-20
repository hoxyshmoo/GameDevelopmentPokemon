using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{

    CharacterAnimator animator;
    public float moveSpeed;
    public bool IsMoving{get; private set;}

    private void Awake(){
        animator=GetComponent<CharacterAnimator>();
    }

    public IEnumerator Move(Vector3 vectorMovement,Action OnMoveOver=null){

        animator.MoveX=Mathf.Clamp(vectorMovement.x,-1f,1f); //New Unity Animator
        animator.MoveY=Mathf.Clamp(vectorMovement.y,-1f,1f);

        var TargetPosition=transform.position;
        TargetPosition.x+=vectorMovement.x;
        TargetPosition.y+=vectorMovement.y;

        if(!IsPathClear(TargetPosition)){
            yield break;
        }

    IsMoving=true;

        while((TargetPosition-transform.position).sqrMagnitude> Mathf.Epsilon){
            transform.position=Vector3.MoveTowards(transform.position,TargetPosition,moveSpeed*Time.deltaTime);
            yield return null;
        
        }
        transform.position=TargetPosition;
        IsMoving=false;

        OnMoveOver?.Invoke();
        //CheckForEncounters();
    }

    public void HandleUpdate(){
        animator.isMoving=IsMoving;
    }

    private bool IsPathClear(Vector3 TargetPosition){
        var diffDistance=TargetPosition-transform.position;
        var direction=diffDistance.normalized;



        if(Physics2D.BoxCast(transform.position+direction,new Vector2(0.2f,0.2f),0f,direction,diffDistance.magnitude-1,GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer)==true){
            return false;
        }
        return true;


    }

 private bool isWalkable(Vector3 TargetPosition){
     if(Physics2D.OverlapCircle(TargetPosition,0.2f,GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer)!=null){  
        return false;
     }
    
     return true;
    }

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

    public CharacterAnimator Animator{
        get => animator;
    }

}
