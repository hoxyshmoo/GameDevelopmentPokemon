using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//Craetes a pokemon party for the player and trainer npc
public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;
    

    public List<Pokemon> Pokemons { get { return pokemons; } }
    
    private void Start()
    {
        //Debug.Log("Pokemon: " + Pokemons.Count);

        foreach (var pokemon in Pokemons)
        {
            pokemon.Init();
            //Debug.Log("Pokemon: "+ pokemon.Base.Name);
        }
    }
    // returns the first healthy pokemon from the party
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}