using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PortalScript : MonoBehaviour, PlayerTriggerable
{

    [SerializeField] int sceneToLoad=-1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationId DestinationPortal;

    PlayerController player;
    Fader fader;


 public void OnplayerTriggered(PlayerController player){
    
    Debug.Log("Player Enter Portal");
    this.player=player;
    StartCoroutine(SwitchScene());
 }

 private void Start(){
    fader=FindObjectOfType<Fader>();

 }


 IEnumerator SwitchScene(){

    DontDestroyOnLoad(gameObject);

    GameController.Instance.pauseGame(true);
   yield return fader.FadeIn(0.5f);

   yield return SceneManager.LoadSceneAsync(sceneToLoad);

   //Find the first portal but excludes the initial portal from previous scene and other portal enum types
   var destinationPortal = FindObjectsOfType<PortalScript>().First(x => x != this && x.DestinationPortal==this.DestinationPortal);
   player.Character.SetPositionToProperTile(destinationPortal.SpawnPoint.position);

   yield return fader.FadeOut(0.5f);

    GameController.Instance.pauseGame(false);

   Destroy(gameObject);

 }

//Get serialized data as a property
 public Transform SpawnPoint => spawnPoint;
}

public enum DestinationId {A,B,C,D,E}