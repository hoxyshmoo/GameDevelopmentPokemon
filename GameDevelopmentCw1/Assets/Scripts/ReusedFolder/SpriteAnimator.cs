using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
SpriteRenderer spriteRender;
List<Sprite> frames;
float frameRate;

int currentFrame;
float timer;

public SpriteAnimator(List<Sprite> frames,SpriteRenderer spriteRender,float frameRate=0.16f){
this.frames=frames;
this.spriteRender=spriteRender;
this.frameRate=frameRate;

}

public void Start(){
    currentFrame=0;
    timer=0f;
    spriteRender.sprite=frames[0];
}

public void HandleUpdate(){
    timer+=Time.deltaTime;
    if(timer>frameRate){
        currentFrame=(currentFrame+1)%frames.Count;
        spriteRender.sprite=frames[currentFrame];
        timer-=frameRate;
    }
}

public List<Sprite> Frames{
    get {return frames;}
}

}
