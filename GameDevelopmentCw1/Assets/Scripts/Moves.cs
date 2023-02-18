using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
