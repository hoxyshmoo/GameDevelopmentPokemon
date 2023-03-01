using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //Initializes the list of pokemon encounters in the wild (grass)
    [SerializeField] List<Pokemon> wildPokemons;

    public Pokemon GetRandomWildPokemon()
    {
        //picks a wild pokemon from the list 
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];
        //Initiate the pokemon
        wildPokemon.Init();
        return wildPokemon;
    }
}