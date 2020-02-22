using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEN_BLACKBOARD : MonoBehaviour
{

    public float wormDetectableRadius = 60; // within this radius worms are detected
    public float wormReachedRadius = 12;    // at this distace worm is eatable
    public float timeToEatWorm = 1.5f;      // it takes this time to eat a worm

    public float chickDetectionRadius = 100;   // within this radius chicks are detected
    public float chickFarEnoughRadius = 250;   // from this distance on chicks stop being an annoyance

    public GameObject attractor;     // hen wanders arounf this point

    public AudioClip angrySound;
    public AudioClip eatingSound;
    public AudioClip cluckingSound;
    
    void Awake()
    {
        attractor = GameObject.Find("Attractor");
        angrySound = Resources.Load<AudioClip>("AngryChicken");
        eatingSound = Resources.Load<AudioClip>("Chew");
        cluckingSound = Resources.Load<AudioClip>("ChickenClucking");
    }

    
}
