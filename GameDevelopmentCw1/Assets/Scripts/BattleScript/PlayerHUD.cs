using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//Displays the HUD for pokemons like name, level health and exp gained.
public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text PkName;
    [SerializeField] Text PkLevel;
    [SerializeField] HP_Bar hp;
    [SerializeField] GameObject expBar;
    Pokemon pokemon;

    // Set data in all HUDs.
    public void SetData(Pokemon pk)
    {
        pokemon = pk;

        PkName.text = pk.Base.Name;
        SetLevel();
        hp.SetHP((float)pk.HP / pk.MaxHP);
        SetExp();
    }
    
    // Update HP for Pokemon Smoothly
    public IEnumerator UpdateHP()
    {
        yield return hp.SetHPSmooth((float)pokemon.HP / pokemon.MaxHP);
    }
    
    //Update Level in HUD
    public void SetLevel()
    {
        PkLevel.text = "Lvl: " + pokemon.Level;
    }

    // Set Exp directly
    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalisedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }
    
    // Increasing EXP Bar smoothly
    public IEnumerator SetExpSmooth(bool reset=false)
    {
        if (expBar == null) yield break;

        if(reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalisedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();

    }

    //Normalizing Exp
    float GetNormalisedExp()
    {
        int currLevelExp = pokemon.Base.GetExpForLevel(pokemon.Level);
        int nextLevelExp = pokemon.Base.GetExpForLevel(pokemon.Level + 1);

        float normalizedExp = (float)(pokemon.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);

    }
}
