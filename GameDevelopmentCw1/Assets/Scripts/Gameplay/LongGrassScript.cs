using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrassScript : MonoBehaviour, PlayerTriggerable
{
    //Script to activate the start battle when walking across long grass layer objects
 public void OnplayerTriggered(PlayerController player){
    

      // Debug.Log("Is this getting called?");
    if(UnityEngine.Random.Range(1,101)<=10){
    
    GameController.Instance.StartBattle();
            }
 }
}
