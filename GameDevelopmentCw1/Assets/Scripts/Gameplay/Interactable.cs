using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface for interact function used in multiple scripts
public interface Interactable
{
    void Interact(Transform initiator);
}
