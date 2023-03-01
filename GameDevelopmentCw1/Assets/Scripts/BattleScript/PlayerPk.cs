using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//class for pokemon in battle secene
public class PlayerPk : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] PlayerHUD hud;

    //Exposing HUD variable
    public PlayerHUD HUD { get { return hud; } }
    
    //Exposing Pokemon variable
    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    // exposing via property
    public bool IsPlayer { get { return isPlayer; } }

    //Setting up the battle scene
    public void CreatePokemon(Pokemon pokemon)
    {
        Pokemon = pokemon;
        hud.gameObject.SetActive(true);
        hud.SetData(Pokemon);

        if (isPlayer)
            image.sprite = Pokemon.Base.Back;
        else
            image.sprite = Pokemon.Base.Front;

        PlayEnterAnimation();
    }
    
    //hiding pokemon hub during player battle scene
    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }

    //Enter Animation usi Dotween
    public void PlayEnterAnimation()
    {
        if (isPlayer)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    //Attack Animation usnig Dotween
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayer)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    //Pokemon hit animation
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));

    }

    //Pokemon faint animation
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

}
