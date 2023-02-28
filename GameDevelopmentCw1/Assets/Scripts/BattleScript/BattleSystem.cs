using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    ActionSelection,
    MoveSelection,
    PerformMove,
    Busy,
    BattleOver,
    PartyScreen
}


public class BattleSystem : MonoBehaviour
{
    //Player
    [SerializeField] PlayerPk playerPk;
    
    //Enemy
    [SerializeField] PlayerPk EnemyPk;
    
    //Dialogs
    [SerializeField] DialogBox dialog;
    
    //PartyScreen
    [SerializeField] PartyScreen partyScreen;

    BattleState state;

    //When Battle is Over
    public event Action<bool> OnBattleOver;

    //Select Action Variable
    int currentAction = 0;
    
    //Select Move Variable
    int currentMove = 0;
    
    //Current Party Member
    int currentMember;
    
    //escape attempts
    int escapeAttempts;

    PokemonParty playerParty;
    Pokemon wildPokemon;

    // Battle Scene starts
    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }
    
    // Coroutine Function for Battle Scene
    public IEnumerator SetupBattle()
    {
        //player
        playerPk.CreatePokemon(playerParty.GetHealthyPokemon());
        //enemy
        EnemyPk.CreatePokemon(wildPokemon);

        //Settings Moves
        dialog.SetMoveNames(playerPk.Pokemon.Moves);

        partyScreen.Init();
        //dialogs
        yield return dialog.TypeDialog($" A wild {EnemyPk.Pokemon.Base.name} appeared!");
        ActionSelection();
        escapeAttempts = 0;
    }

    //Player Pokemon Move
    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;
        var move = playerPk.Pokemon.Moves[currentMove];
        yield return RunMove(playerPk, EnemyPk, move);
        // If state wasn't changed by RunMove function, the Enemy Move
        if (state == BattleState.PerformMove)
            StartCoroutine(EnemyMove());
    }

    // Wild Pokemon or Enemy Pokemon Move
    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;
        var move = EnemyPk.Pokemon.RandomMove();
        yield return RunMove(EnemyPk, playerPk, move);

        if (state == BattleState.PerformMove)
            ActionSelection();

    }

    // Pokemon Move
    IEnumerator RunMove(PlayerPk sourceUnit, PlayerPk targetUnit, Moves move)
    {
        move.PP--;
        yield return dialog.TypeDialog($"{sourceUnit.Pokemon.Base.Name} used {move.Base.Name}.");

        sourceUnit.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        targetUnit.PlayHitAnimation();

        var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
        yield return targetUnit.HUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialog.TypeDialog($"{targetUnit.Pokemon.Base.Name} fainted!");

            targetUnit.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
                        
            if(!targetUnit.IsPlayer) 
            {
                // Exp Gained
                int expYield = targetUnit.Pokemon.Base.ExpYield;
                int faintLevel = targetUnit.Pokemon.Level;
                //float trainerBonus = (isTrainerBattle)? 1.5f : 1f;

                int expGain = Mathf.FloorToInt((expYield * faintLevel /**trainerBonus*/) / 7);

                playerPk.Pokemon.Exp += expGain;

                yield return dialog.TypeDialog($"{sourceUnit.Pokemon.Base.Name} gained {expGain} exp.");
                yield return playerPk.HUD.SetExpSmooth();

                // Check for level up
                while (playerPk.Pokemon.CheckForLevelUp())
                {
                    playerPk.HUD.SetLevel();
                    yield return dialog.TypeDialog($"{playerPk.Pokemon.Base.Name} grew to level {playerPk.Pokemon.Level}.");
                    //Reset Exp Bar
                    yield return playerPk.HUD.SetExpSmooth(true);
                }

                // Time to Exp Gain
                yield return new WaitForSeconds(1f);

            }

            CheckForBattleOver(targetUnit);
        }
    }


    // Checks whose pokemon fainted
    void CheckForBattleOver(PlayerPk faintedUnit)
    {
        if (faintedUnit.IsPlayer)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
                BattleOver(false);
        }
        else
        {
            BattleOver(true);
        }
    }

    // Damage after a move
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return dialog.TypeDialog("A critical hit!");
        if (damageDetails.TypeEffectiveness > 1f)
            yield return dialog.TypeDialog("It's super effective!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return dialog.TypeDialog("It's not very effective!");
    }
    
    // Fight, Bag, Pokemon, Run
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialog.TypeDialog("Choose an action."));
        dialog.EnableSelectAction(true);

    }

    //Pokemon Selection
    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }


    // Pokemon Move Selection
    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialog.EnableSelectAction(false);
        dialog.EnableDiaglogText(false);
        dialog.EnableMoveSelector(true);
    }

    //Setting state to battle over
    void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        OnBattleOver(won);
    }

    // Check which state is active
    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            actionSelect();
        }
        else if (state == BattleState.MoveSelection)
        {
            moveSelect();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartySelection();
        }
    }

    // Selects an action when battle scene starts
    void actionSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            ++currentAction;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentAction += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentAction -= 2;

        currentAction = Mathf.Clamp(currentAction, 0, 3);

        dialog.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                //Fight
                MoveSelection();
            }
            else if (currentAction == 1)
            {
                //Bag will added in future update

            }
            else if (currentAction == 2)
            {
                //Pokemon
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                //Run
                StartCoroutine(TryToEscape());
            }
        }
    }

    //Player Pokemon move selection
    void moveSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            ++currentMove;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMove += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMove -= 2;

        currentMove = Mathf.Clamp(currentMove, 0, playerPk.Pokemon.Moves.Count - 1);

        dialog.UpdateMoveSelection(currentMove, playerPk.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialog.EnableMoveSelector(false);
            dialog.EnableDiaglogText(true);
            StartCoroutine(PlayerMove());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialog.EnableMoveSelector(true);
            dialog.EnableDiaglogText(false);
            actionSelect();
        }
    }

    //Player Party Selection
    void HandlePartySelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            ++currentMember;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            currentMember += 2;
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            currentMember -= 2;

        currentMember = Mathf.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);
        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pokemons[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("Fainted Pokemon cannot battle!");
                return;
            }
            if (selectedMember == playerPk.Pokemon)
            {
                partyScreen.SetMessageText("Pokemon already in battle!");
                return;
            }

            partyScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);
            ActionSelection();
        }
    }

    // Switching to another pokemon in player party
    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        yield return dialog.TypeDialog($"{playerPk.Pokemon.Base.Name}, that's enough!");
        playerPk.PlayFaintAnimation();
        yield return new WaitForSeconds(2f);

        playerPk.CreatePokemon(newPokemon);
        dialog.SetMoveNames(newPokemon.Moves);
        yield return dialog.TypeDialog($"Go {newPokemon.Base.Name} !");

        StartCoroutine(EnemyMove());
    }

    // Run from battle
    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;
        ++escapeAttempts;
        int playerSpeed = playerPk.Pokemon.Speed;
        int enemySpeed = EnemyPk.Pokemon.Speed;

        if (enemySpeed < playerSpeed)
        {
            yield return dialog.TypeDialog($"Ran away safely!");
            BattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialog.TypeDialog($"Ran away safely!");
                BattleOver(true);
            }
            else
            {
                yield return dialog.TypeDialog($"Can't escape!");
                state = BattleState.PerformMove;
            }
        }
    }
}
