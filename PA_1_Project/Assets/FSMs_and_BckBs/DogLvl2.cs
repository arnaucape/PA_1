using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(DogLvl1))]
[RequireComponent(typeof(Dog_Blackboard))]
[RequireComponent(typeof(Flee))]
[RequireComponent(typeof(Seek))]
public class DogLvl2 : FiniteStateMachine
{
    public enum State { INITIAL, LVL1, FINDFOOD, EAT  };
    public State currentState = State.INITIAL;

    private DogLvl1 lvl1;
    private KinematicState ks;
    private Dog_Blackboard bbDog;
    private Seek seek;
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
        seek = GetComponent<Seek>();

        lvl1.enabled = false;
       // flee.enabled = false;
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
        lvl1.enabled = false;
        seek.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        


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
                seek.target = target;
                Debug.Log(target);

                if (SensingUtils.DistanceToTarget(this.gameObject, target)>= bbDog.foodCloseEnoughRadius)
                {
                    
                        ChangeState(State.EAT);
                        break;
                    
                }
               
                break;
            case State.EAT:
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
                seek.enabled = false;
                target = null;
                break;

            case State.EAT:
                //flee.target = target;
                timer = 0;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.enabled = true;
                seek.target = null;
                seek.enabled = false;
                
                break;
            case State.FINDFOOD:
                //flee.target = target;
                seek.enabled = true;
                ks.maxSpeed = 50f;
                ks.maxAcceleration = 25f;
                break;
            case State.EAT:
                //flee.target = target;
                seek.enabled = true;
                ks.maxSpeed = 50f;
                ks.maxAcceleration = 25f;
                break;
        }
        currentState = newState;
    }
}