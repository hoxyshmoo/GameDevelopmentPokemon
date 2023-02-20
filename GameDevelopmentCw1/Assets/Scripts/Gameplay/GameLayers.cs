using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] public LayerMask SolidObjectsLayer; 
    [SerializeField] public LayerMask grassEncounterLayer; 
    [SerializeField] public LayerMask interactableLayer; 
    [SerializeField] public LayerMask playerLayer; 
    [SerializeField] public LayerMask portalLayer; 
    [SerializeField] public LayerMask fovLayer; 

    public static GameLayers i {get; set;}

    private void Awake(){
    i=this;
    }

    public LayerMask SolidLayer{
        get => SolidObjectsLayer;
 
    }
      public LayerMask InteractableLayer{
        get => interactableLayer; 
    }
      public LayerMask GrassLayer{
        get => grassEncounterLayer;
    }
       public LayerMask PlayerLayer{
        get => playerLayer;
    }
       public LayerMask PortalLayer{
        get => portalLayer;
    }
        public LayerMask FovLayer{
        get => fovLayer;
    }
       public LayerMask TriggerableLayer{
        get => grassEncounterLayer | portalLayer;
    }

}
