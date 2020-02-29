using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Blackboard : MonoBehaviour
{
    public float zombieDetectRadius = 60f;
    public float zombieTooClose = 30f;
    
    public float zombieFarEnoughRadius = 70f;

    public float foodDetectionRadius = 80f;
    public float foodFarEnoughRadius = 150f;
    public float foodCloseEnoughRadius = 0f;
    public float eatTime = 2f;

    public string zombieTag = "Zombie";
    public string soundTag = "Sound";
    public string playerTag = "Player";
    public string foodTag = "Food";
}
