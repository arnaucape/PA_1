using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderPlusAvoid))]
[RequireComponent(typeof(FlockingAroundPlusAvoid))]
[RequireComponent(typeof(Zombie_Blackboard))]
public class ZombieLvl1 : FiniteStateMachine
{
    public enum State { INITIAL, WANDER, FLOCK }; //Flock -> all zombies going to the sound act as a flock
    public State currentState = State.INITIAL;

    private WanderPlusAvoid wander;
    private FlockingAroundPlusAvoid flock;
    private KinematicState ks;
    private GameObject bbObject;
    private Zombie_Blackboard bbZombie;
    private DynamicZombie_Blackboard bbInfo;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        wander = GetComponent<WanderPlusAvoid>();
        flock = GetComponent<FlockingAroundPlusAvoid>();
        ks = GetComponent<KinematicState>();
        bbZombie = GetComponent<Zombie_Blackboard>();
        bbInfo = GameObject.Find("DynamicBB").GetComponent<DynamicZombie_Blackboard>();

        flock.enabled = false;
        flock.attractor = null;
        wander.enabled = false;
    }

    public override void Exit()
    {
        // stop any steering that may be enabled
        flock.enabled = false;
        flock.attractor = null;
        wander.enabled = false;
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
                ChangeState(State.WANDER);
                break;
            case State.WANDER:
                target = SensingUtils.FindInstanceWithinRadius(this.gameObject, bbZombie.soundTag, bbZombie.soundDetectableRadius);
                if (target || (bbInfo.soundDetected && SensingUtils.DistanceToTarget(gameObject, bbInfo.soundDetected) <= bbZombie.soundTalkableRadius))
                {
                    ChangeState(State.FLOCK);
                    break;
                }
                break;
            case State.FLOCK:
                if (!bbInfo.soundDetected)
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
            case State.FLOCK:
                flock.enabled = false;
                target = null;
                flock.attractor = null;                
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.WANDER:
                wander.enabled = true;
                break;
            case State.FLOCK:
                if (target) bbInfo.soundDetected = target;
                else target = bbInfo.soundDetected;

                flock.attractor = target;
                flock.enabled = true;
                break;
        }
        currentState = newState;
    }
}