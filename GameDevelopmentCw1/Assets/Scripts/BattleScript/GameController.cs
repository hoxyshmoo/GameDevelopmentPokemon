using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{ FreeRoam, Battle, Dialogue, Paused, Cutscene }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;
    GameState stateB4Pause;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        //  ConditionsDB.Init();
    }

    public void Start()
    {
        //playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.OnEnterTrainerView += (Collider2D trainerCollider) => 
        {
            var trainer = trainerCollider.GetComponentInParent<TrainerController>();
            if (trainer != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(trainer.TriggerTrainerBattle(playerController));
            }
        };

        DialogueManager.Instance.onShowDialogue += () => {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.onCloseDialogue += () => {
            if (state == GameState.Dialogue)
            {
                state = GameState.FreeRoam;
            }

        };
    }
    
    //Handles intermediate transition between portal (no glitches)
    public void pauseGame(bool pause)
    {
        if (pause)
        {
            stateB4Pause = state;
            state = GameState.Paused;
        }
        else
        {
            state = stateB4Pause;
        }
    }

    // Start battle scene for encountring wild pokemon
    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = playerController.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<Grid>().GetComponent<Grid>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    TrainerController trainer;
    // Start battle for trainer
    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        var playerParty = playerController.GetComponent<PokemonParty>();
        var trainerParty = trainer.GetComponent<PokemonParty>();

        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }

    //End battle Scene
    void EndBattle(bool won)
    {
        if(trainer != null && won == true )
        {
            trainer.BattleLost();
            trainer = null;
        }
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    // Update game state
    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
    }


}
