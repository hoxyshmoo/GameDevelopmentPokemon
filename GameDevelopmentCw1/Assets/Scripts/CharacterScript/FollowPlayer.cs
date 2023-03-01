using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    UnityEngine.AI.NavMeshAgent agent;

    private void Awake(){
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation=false;
        agent.updateUpAxis=false;

    }

    private void Start(){
        //target=GameObject.FindWithTag("Player").transform;
      target=GameObject.FindWithTag("Player").transform;
    }

    private void setAgentPosition(){
        agent.SetDestination(new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z));
    }

    private void Update(){
        if(Vector2.Distance(transform.position,target.position)> 2){
          //  transform.position=Vector2.MoveTowards(transform.position,target.position,3*Time.deltaTime);
                 setAgentPosition();
        }
     
      
    }


 
}
