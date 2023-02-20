using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle,
    Dialogue,
    Paused
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem  battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;
    GameState stateB4Pause;

    public static GameController Instance {get; private set;}

    private void Awake(){
        Instance=this;
      //  ConditionsDB.Init();
    }

    public void Start()
    {
        //playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        DialogueManager.Instance.onShowDialogue+=()=>{
            state=GameState.Dialogue;
        };
        
        DialogueManager.Instance.onCloseDialogue+=()=>{
            if(state==GameState.Dialogue){
            state=GameState.FreeRoam;
            }
          
        };
    }
    //Handles intermediate transition between portal (no glitches)
    public void pauseGame(bool pause){
if(pause){
    stateB4Pause=state;
    state=GameState.Paused;

}
else{
state=stateB4Pause;
}
    }

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandelUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandelUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }

    
}
