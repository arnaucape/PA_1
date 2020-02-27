using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderPlusAvoid))]
[RequireComponent(typeof(Dog_Blackboard))]
public class DogLvl1 : FiniteStateMachine
{
    public enum State { INITIAL, WANDER, BARK}; //Flock -> all zombies going to the sound act as a flock
    public State currentState = State.INITIAL;

    private WanderPlusAvoid wander;
    private KinematicState ks;
    private GameObject bbObject;
    private Dog_Blackboard bbDog;
    private GameObject soundWave;
   // private DynamicZombie_Blackboard bbInfo;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        int Childcount = transform.childCount;
        Debug.Log(Childcount);
        soundWave = transform.GetChild(Childcount -1).gameObject;
        wander = GetComponent<WanderPlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbDog = GetComponent<Dog_Blackboard>();
        //bbInfo = GameObject.Find("DynamicBB").GetComponent<DynamicZombie_Blackboard>();

        
       
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
        soundWave.active = false;
        wander.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        wander.enabled = true;
        base.ReEnter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WANDER);
                break;
            case State.WANDER:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieDetectRadius);
                Debug.Log(target);
                if (target !=null)
                {
                    ChangeState(State.BARK);
                    break;
                }
                break;
            case State.BARK:
                if (!SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieDetectRadius))
                {
                    ChangeState(State.WANDER);
                    break;
                }
                break;
        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.INITIAL: break;
            case State.WANDER:
                wander.enabled = false;
                break;
            case State.BARK:
                target = null;
                soundWave.active = false;

                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.WANDER:
                ks.maxAcceleration = 10f;
                ks.maxSpeed = 20f;
                wander.enabled = true;
                break;
            case State.BARK:
                soundWave.active = true;
                break;
        }
        currentState = newState;
    }
}