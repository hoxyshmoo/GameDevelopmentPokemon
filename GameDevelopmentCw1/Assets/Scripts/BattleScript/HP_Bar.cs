using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Health of pokemon is managed here
public class HP_Bar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void SetHP(float hpUpdated)
    {
        health.transform.localScale = new Vector3(hpUpdated, 1f);
    }
    // HP is animated through here
    public IEnumerator SetHPSmooth (float newHP)
    {
        float currentHP = health.transform.localScale.x;
        float changeAmt = currentHP - newHP;

        while( currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(currentHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
