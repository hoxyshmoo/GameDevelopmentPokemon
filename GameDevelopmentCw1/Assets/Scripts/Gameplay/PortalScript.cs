using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, PlayerTriggerable
{

    [SerializeField] int sceneToLoad=-1;

 public void OnplayerTriggered(PlayerController player){
    
    Debug.Log("Player Enter Portal");
    StartCoroutine(SwitchScene());
 }


 IEnumerator SwitchScene(){
   yield return SceneManager.LoadSceneAsync(sceneToLoad);
 }
}
