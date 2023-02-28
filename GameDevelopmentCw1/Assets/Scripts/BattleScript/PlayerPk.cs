using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPk : MonoBehaviour
{
    //[SerializeField] PokemonDB db;
    //[SerializeField] int level;
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

    public bool IsPlayer { get { return isPlayer; } }

    public void CreatePokemon(Pokemon pokemon)
    {
        Pokemon = pokemon;
        hud.SetData(Pokemon);

        if (isPlayer)
            image.sprite = Pokemon.Base.Back;
        else
            image.sprite = Pokemon.Base.Front;

        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayer)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayer)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));

    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }

}
