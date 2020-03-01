using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderPlusAvoid))]
[RequireComponent(typeof(Dog_Blackboard))]
public class DogLvl1 : FiniteStateMachine
{
    //Variable definition
    public enum State { INITIAL, WANDER, BARK}; 
    public State currentState = State.INITIAL;
    private WanderPlusAvoid wander;
    private KinematicState ks;
    private Dog_Blackboard bbDog;
    private GameObject soundWave;
    private DynamicZombie_Blackboard bbInfo;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //Variable instantiation
        int Childcount = transform.childCount;
        soundWave = transform.GetChild(Childcount -1).gameObject;
        wander = GetComponent<WanderPlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbDog = GetComponent<Dog_Blackboard>();
        bbInfo = GameObject.Find("DynamicBB").GetComponent<DynamicZombie_Blackboard>();
    }

    public override void Exit()
    {
        //Check if the variables are instantiated and if they aren't intantiate them.
        //This is done to prevent errors when LVL1.Exit() is executed via another script and the Start function hasn't been executed yet.
        // Stop and disable any steering and intra-behaviour element that may be enabled
        if (soundWave == null)
        {
            int Childcount = transform.childCount;
            soundWave = transform.GetChild(Childcount - 1).gameObject;
        }
        if (bbInfo.soundDetected == null)
        {
            bbInfo = GameObject.Find("DynamicBB").GetComponent<DynamicZombie_Blackboard>();
        }
        if (wander == null)
        {
            wander = GetComponent<WanderPlusAvoid>();
        }
        soundWave.active = false;
        bbInfo.soundDetected = null;
        wander.enabled = false;
        currentState = State.INITIAL;
        base.Exit();
    }

    public override void ReEnter()
    {
        //Same as before. In case the ReEnter() is called by another Script before the Start()
        //Enable the steerings that should be enabled
        currentState = State.INITIAL;
        if (wander == null)
        {
            wander = GetComponent<WanderPlusAvoid>();
        }
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
                //If there is a zombie close enough
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieDetectRadius);
                if (target !=null)
                {
                    ChangeState(State.BARK);
                    break;
                }
                break;

            case State.BARK:
                //If there isn't 
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
        //State enter behaviour
        switch (currentState)
        {
            case State.INITIAL: break;

            case State.WANDER:
                wander.enabled = false;
                break;

            case State.BARK:
                target = null;
                soundWave.active = false;
                bbInfo.soundDetected = null;
                break;
        }

        //State exit behaviour
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