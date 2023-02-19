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

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public PokeType Type { get { return type; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy;} }
    public int PP { get { return pp; } }

}