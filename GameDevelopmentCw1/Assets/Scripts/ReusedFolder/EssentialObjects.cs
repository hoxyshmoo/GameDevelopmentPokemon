using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set property of prefab essential objects to not be destroyed in between scene changes
public class EssentialObjects : MonoBehaviour
{
private void Awake(){
    DontDestroyOnLoad(gameObject); 
}
}
