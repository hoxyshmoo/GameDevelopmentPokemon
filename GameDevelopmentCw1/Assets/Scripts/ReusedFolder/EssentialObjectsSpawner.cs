using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawns the essential object prefab on the scene
public class EssentialObjectsSpawner : MonoBehaviour
{
    //gets the prefab to intialize
[SerializeField] GameObject essentialObjectsPrefab;

private void Awake(){
        //if prefab object doesnt exist, instantiate a new essential object in default position
    var existingObjects= FindObjectsOfType<EssentialObjects>();
    if(existingObjects.Length==0){
        Instantiate(essentialObjectsPrefab,new Vector3(0,0,0),Quaternion.identity);
    }
    else{
            //if the prefab object already destroy the object
            Destroy(gameObject);
    }
}
}
