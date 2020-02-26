using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Blackboard : MonoBehaviour
{
    public float soundDetectableRadius = 60f;
    public float soundTalkableRadius = 120f; //radius where others zombies can recieve the info of a sound, by another zombie

    public float playerDetectionRadius = 80f;
    public float playerFarEnoughRadius = 150f;

    public string zombieTag = "Zombie";
    public string soundTag = "Sound";
    public string playerTag = "Player";
}
