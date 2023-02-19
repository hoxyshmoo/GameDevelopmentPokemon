using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

//Sprites for Animation
[SerializeField] List<Sprite> wDownSprite;
[SerializeField] List<Sprite> wUpSprite;
[SerializeField] List<Sprite> wRightSprite;
[SerializeField] List<Sprite> wLeftSprite;

    //Parameters for Moving
    public float MoveX {get; set;}
    public float MoveY {get; set;}
    public bool isMoving {get; set;}

    //States for Animation
    SpriteAnimator wDownAnimation;
    SpriteAnimator wUpAnimation;
    SpriteAnimator wRightAnimation;
    SpriteAnimator wLeftAnimation;

    SpriteAnimator currentAnimationState;

    bool wasPrevMoving;

    //Reference Class
    SpriteRenderer spriteRend;

    private void Start(){
        spriteRend=GetComponent<SpriteRenderer>();

        wDownAnimation=new SpriteAnimator(wDownSprite,spriteRend); 
        wUpAnimation=new SpriteAnimator(wUpSprite,spriteRend); 
        wRightAnimation=new SpriteAnimator(wRightSprite,spriteRend); 
        wLeftAnimation=new SpriteAnimator(wLeftSprite,spriteRend); 

        currentAnimationState=wDownAnimation;
    }

    private void Update(){

        var prevAnimationState = currentAnimationState;

        if(MoveX==1){
            currentAnimationState=wRightAnimation;
        }
        else if(MoveX==-1){
            currentAnimationState=wLeftAnimation;
        }
        else if(MoveY==1){
            currentAnimationState=wUpAnimation;
        }
        else if(MoveY==-1){
            currentAnimationState=wDownAnimation;
        }

        if(currentAnimationState!=prevAnimationState || isMoving != wasPrevMoving){
            currentAnimationState.Start();
        }

        if(isMoving){
            currentAnimationState.HandleUpdate();
        }
        else{
            spriteRend.sprite=currentAnimationState.Frames[0];
        }
        wasPrevMoving=isMoving; //Fixes Character Sliding
    }
}