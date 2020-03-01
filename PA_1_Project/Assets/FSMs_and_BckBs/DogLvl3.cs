using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(DogLvl2))]
[RequireComponent(typeof(Dog_Blackboard))]
[RequireComponent(typeof(FleePlusAvoid))]
public class DogLvl3 : FiniteStateMachine
{
    public enum State { INITIAL, LVL2, FLEE };
    public State currentState = State.INITIAL;

    private DogLvl2 lvl2;
    private KinematicState ks;
    private Dog_Blackboard bbDog;
    private FleePlusAvoid flee;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        lvl2 = GetComponent<DogLvl2>();
        flee = GetComponent<FleePlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbDog = GetComponent<Dog_Blackboard>();
        lvl2.enabled = false;
        flee.enabled = false;
    }

    public override void Exit()
    {
        //Stop any steering that may be enabled
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
                ChangeState(State.LVL2);
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
                if (target == null)
                {
                    ChangeState(State.LVL2);
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

            case State.LVL2:
                lvl2.enabled = true;
                flee.target = null;
                flee.enabled = false;
                lvl2.ReEnter();
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