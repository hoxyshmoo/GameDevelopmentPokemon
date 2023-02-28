using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonDB _base;
    [SerializeField] int level;

    // Exposing Private Variables
    public PokemonDB Base { get { return _base; } }

    public int Level { get { return level; } }

    public int HP { get; set; }

    public List<Moves> Moves { get; set; }

    public int Exp { get; set; }
    
    // Initialization Function 
    public void Init()
    {
        HP = MaxHP;

        // Generate move
        Moves = new List<Moves>();
        foreach (var move in Base.NewMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Moves(move.PML));
            }
            if (Moves.Count >= 4)
                break;
        }

        Exp = Base.GetExpForLevel(Level);
    }

    // Function to check for level up for a pokemon
    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        return false;
    }

    //Setting values and exposing variables for base stat of pokemon
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defence
    {
        get { return Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefence
    {
        get { return Mathf.FloorToInt((Base.SpDefence * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }

    //Calculating Damage for a Pokemon
    public DamageDetails TakeDamage(Moves move, Pokemon attacker)
    {
        //critical hits
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;
        //Pokemon Move TypeEffectiveness
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defence = (move.Base.IsSpecial) ? attacker.SpDefence : attacker.SpDefence;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defence);
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }
    
    // Wild Pokemon AI
    public Moves RandomMove(int HP)
    {
        int currentHP = HP;
        int r = 0;
        if (currentHP >= (MaxHP/0.25))
        {
            r = Random.Range(0, Moves.Count);
        }
        else if (currentHP >= (MaxHP/0.5))
        {
            r = Random.Range(1, Moves.Count);
        }
        else if( currentHP >= (MaxHP/0.66))
        {
            r = Random.Range(2, Moves.Count);
        }
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Critical { get; set; }

    public float TypeEffectiveness { get; set; }

}