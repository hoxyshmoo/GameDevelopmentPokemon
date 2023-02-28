using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create New Move")]
public class PokemonMovesList : ScriptableObject
{
    [SerializeField] new string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] PokeType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;
    
    // Exposing the the private variables
    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public PokeType Type { get { return type; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy;} }
    public int PP { get { return pp; } }
    // Is the move Special or Physical
    public bool IsSpecial
    {
        get
        {
            if (type == PokeType.Fire || type == PokeType.Water || type == PokeType.Electric || type == PokeType.Grass || type == PokeType.Ice || type == PokeType.Dragon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
