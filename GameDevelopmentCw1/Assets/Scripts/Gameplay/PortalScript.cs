using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PortalScript : MonoBehaviour, PlayerTriggerable
{
    //set default portal to -1 (no portal scene)
    [SerializeField] int sceneToLoad=-1;
    //set spawn point for given portal
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationId DestinationPortal;

    PlayerController player;
//fader for dotween in between scene (fade and unfade to black)
    Fader fader;

    //When player triggers portal, start the switch scene code
 public void OnplayerTriggered(PlayerController player){
    
    Debug.Log("Player Enter Portal");
    this.player=player;
    StartCoroutine(SwitchScene());
 }
    //initialize fader object 
 private void Start(){
    fader=FindObjectOfType<Fader>();

 }

    //Switch scenes function
 IEnumerator SwitchScene(){

    DontDestroyOnLoad(gameObject);
        //pause the npc game controller between switching scenes
    GameController.Instance.pauseGame(true);
   yield return fader.FadeIn(0.5f);
        //waits till scene is loaded before returning scene to load
   yield return SceneManager.LoadSceneAsync(sceneToLoad);

   //Find the first portal but excludes the initial portal from previous scene and other portal enum types
   var destinationPortal = FindObjectsOfType<PortalScript>().First(x => x != this && x.DestinationPortal==this.DestinationPortal);
   player.Character.SetPositionToProperTile(destinationPortal.SpawnPoint.position);

   yield return fader.FadeOut(0.5f);

        //resume game and unpause the game controller for npc
    GameController.Instance.pauseGame(false);

   Destroy(gameObject);

 }

//Get serialized data as a property
 public Transform SpawnPoint => spawnPoint;
}
//get destination id enum (A only goes to portal A, B only goes to portal B and etc)
public enum DestinationId {A,B,C,D,E}