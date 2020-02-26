using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(DogLvl1))]
[RequireComponent(typeof(DogLvl2))]
[RequireComponent(typeof(Dog_Blackboard))]
[RequireComponent(typeof(Flee))]
public class DogLvl3 : FiniteStateMachine
{
    public enum State { INITIAL, LVL1, LVL2, FLEE };
    public State currentState = State.INITIAL;

    private DogLvl1 lvl1;
    private DogLvl2 lvl2;
    private KinematicState ks;
    private Dog_Blackboard bbDog;
    private FleePlusAvoid flee;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        lvl1 = GetComponent<DogLvl1>();
        lvl2 = GetComponent<DogLvl2>();
        flee = GetComponent<FleePlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbDog = GetComponent<Dog_Blackboard>();

        lvl1.enabled = false;
        lvl2.enabled = false;
        flee.enabled = false;
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
        lvl1.enabled = false;
        lvl2.enabled = false;
        flee.enabled = false;
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
                if (lvl1.enabled)
                {
                    ChangeState(State.LVL1);
                }
                else
                    ChangeState(State.LVL2);

                break;
            case State.LVL1:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieTooClose);
                if (target != null)
                {
                    ChangeState(State.FLEE);
                    break;
                }
                break;
            case State.LVL2:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieTooClose);
                if (target != null)
                {
                    ChangeState(State.FLEE);
                    break;
                }
                break;
            case State.FLEE:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbDog.zombieTag, bbDog.zombieDetectRadius);
                flee.target = target;
                Debug.Log(target);
                if (target == null)
                {
                    
                        ChangeState(State.LVL1);
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
            case State.LVL1:
                lvl1.Exit();
                break;
            case State.LVL2:
                lvl2.Exit();
                break;
            case State.FLEE:
                flee.enabled = false;
                target = null;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.enabled = true;
                flee.target = null;
                flee.enabled = false;
                break;

            case State.LVL2:
                lvl2.enabled = true;
                flee.target = null;
                flee.enabled = false;

                break;
            case State.FLEE:
                //flee.target = target;
                flee.enabled = true;
                ks.maxSpeed = 50f;
                ks.maxAcceleration = 25f;
                break;
        }
        currentState = newState;
    }
}