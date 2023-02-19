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
    BattleOver
}


public class BattleSystem : MonoBehaviour
{
    //Player
    [SerializeField] PlayerPk playerPk;
    //[SerializeField] PlayerHUD playerHUD;
    
    //Enemy
    [SerializeField] PlayerPk EnemyPk;
    //[SerializeField] PlayerHUD EnemyHUD;
    
    //Dialogs
    [SerializeField] DialogBox dialog;

    BattleState state;

    //When Battle is Over
    public event Action<bool> OnBattleOver;
    
    //Select Action Variable
    int currentAction = 0;
    //Select Move Variable
    int currentMove = 0;

    //escape attempts
    int escapeAttempts;

    // Battle Scene starts
    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }
    // Coroutine Function for Battle Scene
    public IEnumerator SetupBattle()
    {
        //player
        playerPk.CreatePokemon();
        //playerHUD.SetData(playerPk.Pokemon);
        //enemy
        EnemyPk.CreatePokemon();
        //EnemyHUD.SetData(EnemyPk.Pokemon);
        //Settings Moves
        dialog.SetMoveNames(playerPk.Pokemon.Moves);
        //dialogs
        yield return dialog.TypeDialog($" A wild {EnemyPk.Pokemon.Base_db.name} appeared !!!");
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
        if(state == BattleState.PerformMove)
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
        yield return dialog.TypeDialog($"{sourceUnit.Pokemon.Base_db.Name} used {move.Base.Name} !");

        var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
        yield return targetUnit.HUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return dialog.TypeDialog($"{targetUnit.Pokemon.Base_db.Name} fainted !");

            yield return new WaitForSeconds(2f);
            //OnBattleOver(true);
            // Gain exp
            int exp = targetUnit.Pokemon.Base_db.ExpYield;
            int faintLevel = targetUnit.Pokemon.Level;

            int expGain = (exp * faintLevel) / 7;

            playerPk.Pokemon.Exp += expGain;

            yield return dialog.TypeDialog($"{sourceUnit.Pokemon.Base_db.Name} gained {expGain} exp.");
            // Check for level up

            CheckForBattleOver(targetUnit);
        }
    }


    // Checks whose pokemon fainted
    void CheckForBattleOver(PlayerPk faintedUnit)
    {
        if (faintedUnit.IsPlayer)
        {
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
    // Fight or Run
    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialog.TypeDialog("Choose an action."));
        dialog.EnableSelectAction(true);
    }
    // Pokemon Move Selection
    void MoveSelection()
    {
        state= BattleState.MoveSelection;
        dialog.EnableSelectAction(false);
        dialog.EnableDiaglogText(false);
        dialog.EnableMoveSelector(true);
    }
    

    void BattleOver( bool won)
    {
        state= BattleState.BattleOver;
        OnBattleOver(won);
    }

    public void HandelUpdate()
    {
        if(state == BattleState.ActionSelection)
        {
            actionSelect();
        }
        else if (state == BattleState.MoveSelection)
        {
            moveSelect();
        }
    }

    void actionSelect()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            {
                ++currentAction;
            }
                
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }

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
                //Run
                StartCoroutine( TryToEscape());
            } 
        }
    }

    void moveSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < playerPk.Pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerPk.Pokemon.Moves.Count - 1)
            {
                currentMove+=2;
            }

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove-=2;
            }
        }

        dialog.UpdateMoveSelection(currentMove, playerPk.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialog.EnableMoveSelector(false);
            dialog.EnableDiaglogText(true);
            StartCoroutine(PlayerMove());
        }
    }
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

            if (UnityEngine.Random.Range(0,256) < f)
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
