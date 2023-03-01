using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// how pokemon looks in party screen
public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text PkName;
    [SerializeField] Text PkLevel;
    [SerializeField] HP_Bar hp;

    [SerializeField] Color highlightedColor;

    Pokemon pokemon;

    // Set data in all HUDs.
    public void SetData(Pokemon pk)
    {
        pokemon = pk;

        PkName.text = pk.Base.Name;
        PkLevel.text = "Lvl: " + pk.Level;
        hp.SetHP((float)pk.HP / pk.MaxHP);
    }
    // highlights Color for the selected pokemon
    public void SetSelected(bool selected)
    {
        if (selected)
            PkName.color = highlightedColor;
        else
            PkName.color = Color.black;
    }
}
