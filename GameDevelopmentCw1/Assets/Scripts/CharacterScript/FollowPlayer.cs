using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nav mesh for pokemon to follow player
public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    UnityEngine.AI.NavMeshAgent agent;

    private void Awake(){
        //assigning the nav mesh agent script to agent variable
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        //using nav mesh but lock the axis and rotation for 2d environment
        agent.updateRotation=false;
        agent.updateUpAxis=false;

    }

    private void Start(){
        //target=GameObject.FindWithTag("Player").transform;
        //find the target object using tag and convert it into a transform type variable
      target=GameObject.FindWithTag("Player").transform;
    }

    //function to update the destination of the agent given the target's current position
    private void setAgentPosition(){
        agent.SetDestination(new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z));
    }

    //updat the nav mesh agent location when the distance is less than 2 
    private void Update(){
        if(Vector2.Distance(transform.position,target.position)> 2){
          //  transform.position=Vector2.MoveTowards(transform.position,target.position,3*Time.deltaTime);
                 setAgentPosition();
        }
     
      
    }


 
}
