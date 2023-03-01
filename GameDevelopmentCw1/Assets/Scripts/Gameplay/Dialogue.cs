using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make the dialogue serializable
[System.Serializable]

public class Dialogue 
{
//get dialogue list
[SerializeField] List<string> lines;

//return dialogue list
public List<string> Lines{
    get {return lines;}
}
}
