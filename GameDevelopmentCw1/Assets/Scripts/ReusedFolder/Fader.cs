using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Fader objct for dotween animawtion between scene and pokemon start battle scenes
public class Fader : MonoBehaviour
{
   Image img;
   
   private void Awake(){
    img=GetComponent<Image>();
   }

 public IEnumerator FadeIn(float time){

        yield return img.DOFade(1f, time).WaitForCompletion();
 }
   
   public IEnumerator FadeOut(float time){
    yield return img.DOFade(0f,time).WaitForCompletion();
   }
}
