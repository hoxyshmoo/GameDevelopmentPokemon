using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    [SerializeField] new string name;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject exclamation;
    [SerializeField] Dialogue dialog;
    [SerializeField] Dialogue dialogAfterBattle;
    [SerializeField] GameObject fov;

    //NpcScript move;

    // State
    bool battleLost = false;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
       // List<Vector2> move= GetComponent<NpcScript>();
    }

    private void Start()
    {
        //SetFOVRotation(move.MovePattern);
        SetFOVRotation(character.Animator.DefaultDirection);
        //Debug.Log("MovePattern: " + move.MovePattern);
    }

    public void Interact(Transform initiator)
    {
        character.LookAttention(initiator.position);
        // Show diaglog, battle scene starts
        if (!battleLost) 
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialog, () =>
            {
                GameController.Instance.StartTrainerBattle(this);
            }));
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogAfterBattle));
        }
        
    }

    // Triggers Trainer Battle
    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        // Show Exclamation
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        // Walk towards the player
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);

        // Show diaglog
        StartCoroutine(DialogueManager.Instance.ShowDialogue(dialog, () =>
        {
            GameController.Instance.StartTrainerBattle(this);
        }));
    }

    // FOV rotation; as trainer moves/rotates so does the fov
    public void SetFOVRotation(FacingDirection dir)
    {
        float angle = 0f;

        if (dir == FacingDirection.Right)
            angle = 90f;
        else if (dir == FacingDirection.Up)
            angle = 180f;
        else if (dir == FacingDirection.Left)
            angle = 270f;

        //Setting Z axis value to FOC of Boss
        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }


    //Battle lost
    public void BattleLost()
    {
        battleLost = true;
        fov.gameObject.SetActive(false);
    }

    // Exposing variables via creating properties
    public string Name { 
        get => name; 
    }

    public Sprite Sprite { 
        get => sprite;
    }

}
