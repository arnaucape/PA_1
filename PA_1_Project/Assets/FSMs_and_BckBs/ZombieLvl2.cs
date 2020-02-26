using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(ZombieLvl1))]
[RequireComponent(typeof(Zombie_Blackboard))]
[RequireComponent(typeof(Seek))]
public class ZombieLvl2 : FiniteStateMachine
{
    public enum State { INITIAL, LVL1, SEEK };
    public State currentState = State.INITIAL;

    private ZombieLvl1 lvl1;
    private Seek seek;
    private KinematicState ks;
    private Zombie_Blackboard bbZombie;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        lvl1 = GetComponent<ZombieLvl1>();
        seek = GetComponent<Seek>();
        ks = GetComponent<KinematicState>();
        bbZombie = GetComponent<Zombie_Blackboard>();

        lvl1.enabled = false;
        seek.enabled = false;
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
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbZombie.playerTag, bbZombie.playerDetectionRadius);
                if (target)
                {
                    ChangeState(State.SEEK);
                    break;
                }
                break;
            case State.SEEK:
                if (SensingUtils.DistanceToTarget(this.gameObject, target) > bbZombie.playerFarEnoughRadius)
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
            case State.SEEK:
                seek.enabled = false;
                target = null;
                seek.target = null;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.enabled = true;
                break;
            case State.SEEK:
                seek.target = target;
                seek.enabled = true;
                break;
        }
        currentState = newState;
    }
}