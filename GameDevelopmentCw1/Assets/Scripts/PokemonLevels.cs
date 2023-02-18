using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonLevels
{
    public PokemonDB Base_db { get; set; }
    
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Moves> Moves { get; set; }

    public int Exp { get; set; }

    public PokemonLevels(PokemonDB DB, int lvl)
    {
        Base_db = DB;
        Level = lvl;
        HP = MaxHP;

        //generate move
        Moves = new List<Moves> ();
        foreach (var move in Base_db.NewMoves) 
        { 
            if(move.Level <= Level ) 
            {
             Moves.Add(new Moves(move.PML)); 
            }
            if (Moves.Count >= 4)
                break;
        }

        Exp = Base_db.GetExpForLevel(Level);
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base_db.Attack * Level) / 100f) + 5; }
    }
    public int Defence
    {
        get { return Mathf.FloorToInt((Base_db.Defence * Level) / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base_db.SpAttack * Level) / 100f) + 5; }
    }
    public int SpDefence
    {
        get { return Mathf.FloorToInt((Base_db.SpDefence * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base_db.Speed * Level) / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base_db.MaxHP * Level) / 100f) + 10; }
    }

    public DamageDetails TakeDamage(Moves move, PokemonLevels attacker)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base_db.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base_db.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false
        };

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defence) ;
        int damage = Mathf.FloorToInt( d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted= true;
        }
        return damageDetails;
    }

    public Moves RandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Critical { get; set; }

    public float TypeEffectiveness { get; set; }

}