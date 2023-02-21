using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    private void Start(){
        target=GameObject.FindWithTag("Player").transform;

    }

    private void Update(){
        if(Vector2.Distance(transform.position,target.position)> 2){
            transform.position=Vector2.MoveTowards(transform.position,target.position,3*Time.deltaTime);
        }
    }
 
}
