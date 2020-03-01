using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(DogLvl1))]
[RequireComponent(typeof(Dog_Blackboard))]
[RequireComponent(typeof(FleePlusAvoid))]
[RequireComponent(typeof(ArrivePlusAvoid))]
public class DogLvl2 : FiniteStateMachine
{
    public enum State { INITIAL, LVL1, FINDFOOD, EAT  };
    public State currentState = State.INITIAL;

    private DogLvl1 lvl1;
    private KinematicState ks;
    private Dog_Blackboard bbDog;
    private ArrivePlusAvoid arrive;
    private float timer;
   // private FleePlusAvoid flee;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        lvl1 = GetComponent<DogLvl1>();
        //flee = GetComponent<FleePlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbDog = GetComponent<Dog_Blackboard>();
        arrive = GetComponent<ArrivePlusAvoid>();
        lvl1.enabled = false;
        arrive.enabled = false;
       // flee.enabled = false;
    }

    public override void Exit()
    {
        lvl1.Exit();
        // stop any steering that may be enabled
        lvl1.enabled = false;
        arrive.target = null;
        arrive.enabled = false;
        currentState = State.INITIAL;
        base.Exit();
    }

    public override void ReEnter()
    {
        arrive.enabled = false;
        arrive.target = null;
        target = null;
        currentState = State.INITIAL;
        base.ReEnter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:

                ChangeState(State.LVL1);
                break;
            case State.LVL1:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.foodTag, bbDog.foodDetectionRadius);
                if (target != null)
                {
                    ChangeState(State.FINDFOOD);
                    break;
                }
                break;
               
            case State.FINDFOOD:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.foodTag, bbDog.foodDetectionRadius);
                if (target != null)
                {
                    arrive.target = target;
                
                    if (SensingUtils.DistanceToTarget(this.gameObject, target) <= bbDog.foodCloseEnoughRadius)
                    {

                        ChangeState(State.EAT);

                        break;

                    }
                }
                else
                {
                    ChangeState(State.LVL1);
                    break;
                }
                break;


            case State.EAT:
                Debug.Log(timer);
               
                if (timer >= bbDog.eatTime)
                {
                   
                    ChangeState(State.LVL1);
                    break;

                }
                else
                {
                    timer += Time.deltaTime;
                }

                break;
        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.Exit();
                break;
            case State.FINDFOOD:
                arrive.enabled = false;
                target = null;
                break;

            case State.EAT:
                Destroy(arrive.target);
                timer = 0;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.enabled = true;
                //seek.target = null;
                arrive.enabled = false;
                lvl1.ReEnter();
                
                break;
            case State.FINDFOOD:
                //flee.target = target;
                arrive.enabled = true;
                ks.maxSpeed = 50f;
                ks.maxAcceleration = 5f;
                break;
            case State.EAT:
                //flee.target = target;
                arrive.enabled = true;
                ks.maxSpeed = 50f;
                ks.maxAcceleration = 25f;
                break;
        }
        currentState = newState;
    }
}