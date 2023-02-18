using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPk : MonoBehaviour
{
    [SerializeField] PokemonDB db;
    [SerializeField] int level;
    [SerializeField] bool isPlayer;
    [SerializeField] PlayerHUD hud;

    public PlayerHUD HUD 
    { 
        get { return hud; } 
    }

    public PokemonLevels Pokemon { get; set; }
    
    public bool IsPlayer
    {
        get { return isPlayer;}
    }

    public void CreatePokemon()
    {
        Pokemon = new PokemonLevels(db,level);
        hud.SetData(Pokemon);

        if (isPlayer)
            GetComponent<Image>().sprite = Pokemon.Base_db.Back;
        else
            GetComponent<Image>().sprite = Pokemon.Base_db.Front;

        
    }
}
