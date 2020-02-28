using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor_Blackboard : MonoBehaviour
{
    public GameObject zombieDetected;
    public string zombieTag;
    public float zombieCloseEnough;
    public float zombieFarEnough;
    public float repulsionForce;

    public GameObject hideout;
    public float hideoutRadius;

    public GameObject resourcesFound;
    public float resourcesSensingRadius;
    public float pickupRadius;
    public string resourcesTag;

    public bool insideHideout;
    public float waitDuration;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
