using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideoutScript : MonoBehaviour
{
    public GameObject survivor;
    public float hideoutRadius;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SensingUtils.DistanceToTarget(this.gameObject, survivor) < hideoutRadius)
        {
            if (survivor.tag != "Untagged")
                survivor.tag = "Untagged";
        }
        else
        {
            if (survivor.tag != "Player")
                survivor.tag = "Player";
        }

    }
}
