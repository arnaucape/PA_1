using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(ZombieLvl1))]
[RequireComponent(typeof(Zombie_Blackboard))]
[RequireComponent(typeof(FlockingAroundPlusAvoid))]
public class ZombieLvl2 : FiniteStateMachine
{
    public enum State { INITIAL, LVL1, FLOCK };
    public State currentState = State.INITIAL;

    private ZombieLvl1 lvl1;
    private FlockingAroundPlusAvoid flock;
    private KinematicState ks;
    private Zombie_Blackboard bbZombie;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        lvl1 = GetComponent<ZombieLvl1>();
        flock = GetComponent<FlockingAroundPlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbZombie = GetComponent<Zombie_Blackboard>();

        lvl1.enabled = false;
        flock.enabled = false;
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
        lvl1.enabled = false;
        flock.enabled = false;
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
                    ChangeState(State.FLOCK);
                    break;
                }
                break;
            case State.FLOCK:
                if (SensingUtils.DistanceToTarget(this.gameObject, target) > bbZombie.playerFarEnoughRadius
                    || target.tag != bbZombie.playerTag)
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
            case State.FLOCK:
                flock.enabled = false;
                target = null;
                flock.attractor = null;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.LVL1:
                lvl1.ReEnter();
                break;
            case State.FLOCK:
                flock.attractor = target;
                flock.enabled = true;
                break;
        }
        currentState = newState;
    }
}