using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public float speed;
  public float checkRadius;
 // public float attackRadius;
  public bool shouldRotate;
  public LayerMask whatIsPlayer;
  public Vector3 direction;

  private Transform target;
  private Rigidbody2D rb;
  private Animator anim;
  private Vector2 movement;
  private bool isInChaseRange;
  //private bool isInAttackRange;

  private void Start(){
    rb=GetComponent<Rigidbody2D>();
    anim=GetComponent<Animator>();
    target=GameObject.FindWithTag("Player").transform; 

  }

  private void Update(){
    anim.SetBool("isRunning",isInChaseRange);
    isInChaseRange=Physics2D.OverlapCircle(transform.position,checkRadius,whatIsPlayer);
    direction=target.position-transform.position;
    float angle=Mathf.Atan2(direction.y,direction.x)*Mathf.Rad2Deg;
    direction.Normalize();
    movement=direction;
    if(shouldRotate){
        anim.SetFloat("X",direction.x);
         anim.SetFloat("Y",direction.y);
    }
  }

  private void FixedUpdate(){
if(isInChaseRange){
    MoveCharacter(movement);
}
  }

  private void MoveCharacter(Vector2 dir){

    if(Vector2.Distance(transform.position,target.position)> 2){
                rb.MovePosition((Vector2)transform.position+(dir*speed*Time.deltaTime));
        }
        else{
            anim.SetBool("isRunning",false);
        }

  }
}
