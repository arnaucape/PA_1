
using UnityEngine;

public class DieNear : MonoBehaviour
{

    public GameObject cemetery;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SensingUtils.DistanceToTarget(this.gameObject, cemetery) < 10)
            Object.Destroy(this.gameObject);
    }
}
