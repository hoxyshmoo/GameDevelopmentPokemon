using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text PkName;
    [SerializeField] Text PkLevel;
    [SerializeField] HP_Bar hp;
    [SerializeField] GameObject expBar;
    PokemonLevels pokemon;
    
    // Set data in all HUDs.
    public void SetData (PokemonLevels pk)
    {
        pokemon= pk;

        PkName.text = pk.Base_db.Name;
        PkLevel.text = "Lvl: "+pk.Level;
        hp.SetHP((float) pk.HP/pk.MaxHP);
        SetExp();
    }

    public IEnumerator UpdateHP ()
    {
        yield return hp.SetHPSmooth((float)pokemon.HP / pokemon.MaxHP);
    }

    public void SetExp () 
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalisedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }

    float GetNormalisedExp()
    {
        int currLevelExp = pokemon.Base_db.GetExpForLevel(pokemon.Level);
        int nextLevelExp = pokemon.Base_db.GetExpForLevel(pokemon.Level +1);

        float normalizedExp = (float)(pokemon.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);

    }
}
