    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        public LayerMask SolidObjectsLayer; 
    public float moveSpeed;
private Animator animator;
    private bool isMoving;
    private Vector2 input;

    private void Awake(){
        animator=GetComponent<Animator>(); 
    }

    private void Update(){
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
    }

    IEnumerator Move(Vector3 TargetPosition){

    isMoving=true;

        while((TargetPosition-transform.position).sqrMagnitude> Mathf.Epsilon){
            transform.position=Vector3.MoveTowards(transform.position,TargetPosition,moveSpeed*Time.deltaTime);
            yield return null;
        
        }
        transform.position=TargetPosition;
        isMoving=false;
    }

    private bool isWalkable(Vector3 TargetPosition){
     if(Physics2D.OverlapCircle(TargetPosition,0.2f,SolidObjectsLayer)!=null){
        return false;
     }
     return true;
    }

    }
