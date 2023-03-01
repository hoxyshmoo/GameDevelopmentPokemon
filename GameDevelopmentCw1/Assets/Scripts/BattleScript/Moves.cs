using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List of moves for pokemon
public class Moves
{
    public PokemonMovesList Base { get; set; }
    public int PP { get; set; }

    public Moves (PokemonMovesList pBase )
    {
        Base = pBase; 
        PP = pBase.PP;
    }
}
