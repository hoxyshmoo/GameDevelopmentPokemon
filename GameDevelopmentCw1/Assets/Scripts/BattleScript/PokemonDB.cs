using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create New Pokemon")]
public class PokemonDB : ScriptableObject
{
    //Pokemon details
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] PokeType type1;
    [SerializeField] PokeType type2;

    //Base Stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defence;    
    [SerializeField] int spAttack;
    [SerializeField] int spDefence;
    [SerializeField] int speed;
    
    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] List<newMoves> newMoves;

    //exposing private variable using properties
    public string Name { get { return name; } }
    public string Description{ get { return description; }     }
    public Sprite Front { get { return front; } }
    public Sprite Back { get { return back; } }
    public PokeType Type1 { get { return type1; } }
    public PokeType Type2 { get { return type2; } }
    public int MaxHP { get { return maxHP; } }
    public int Attack { get { return attack;} }
    public int Defence { get { return defence;} }
    public int SpAttack { get { return spAttack; } }
    public int SpDefence { get { return spDefence;} }
    public int Speed { get { return speed;} }
    public int ExpYield { get { return expYield; } }
    public GrowthRate GrowthRate { get { return growthRate; } }
    public List<newMoves> NewMoves { get { return newMoves; } }

    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
            return ((4*level*level*level)/5);
        else if (growthRate == GrowthRate.MediumFast)
            return (level*level*level);

        return -1;
    }
}

//
[System.Serializable]
public class newMoves
{
    [SerializeField] PokemonMovesList pml;
    [SerializeField] int level;

    public PokemonMovesList PML { get { return pml; } }
    public int Level { get { return level; } }
}

//Pokemon GrowthRate
public enum GrowthRate
{
    Fast, MediumFast
}

//Pokemon Types
public enum PokeType {
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dark,
    Dragon,
    Steel,
    Fairy
}

// How diffrent types does damage to one another
public class TypeChart
{
    static float[][] chart =
    {
        //                  NOR  FIR   WAT   ELE  	GRA   ICE   FIG  POI
        /*NOR*/ new float[] {1f, 1f,   1f,   1f,    1f,   1f,   1f,  1f},
        /*FIR*/ new float[] {1f, 0.5f, 0.5f, 1f,    2f,   2f,   1f,  1f},
        /*WAT*/ new float[] {1f, 2f,   0.5f, 1f,    0.5f, 1f,   1f,  1f},
        /*ELE*/ new float[] {1f, 1f,   2f,   0.5f,  0.5f, 1f,   1f,  1f},
        /*GRA*/ new float[] {1f, 0.5f, 2f,   1f,    0.5f, 1f,   1f,  0.5f},
        /*ICE*/ new float[] {1f, 0.5f, 0.5f, 1f,    2f,   0.5f, 1f,  1f},
        /*FIG*/ new float[] {2f, 1f,   1f,   1f,    1f,   2f,   1f,  0.5f},
        /*POI*/ new float[] {1f, 1f,   1f,   1f,    1f,   1f,   1f,  0.5f}

    };

    public static float GetEffectiveness(PokeType attackType, PokeType defenseType)
    {
        if (attackType == PokeType.None || defenseType == PokeType.None) 
        { 
            return 1;
        }

        int row = (int)attackType -1;
        int col = (int)defenseType -1;
        Debug.Log("row: " + row);
        Debug.Log("col: " + col);
        return chart[row][col];
    }
}
