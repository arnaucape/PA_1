using UnityEngine;
using FSM;
using Steerings;

[RequireComponent(typeof(WanderAround), typeof(Arrive), typeof(Survivor_Blackboard))]
public class FSM_GatherResources : FiniteStateMachine
{
    public enum State { INITIAL, WANDER, GOTO_RESOURCES, GOTO_HIDEOUT, WAIT};
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Arrive arrive;
    private Survivor_Blackboard blackboard;

    private float waitTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<Survivor_Blackboard>();

        wanderAround.attractor = blackboard.hideout;
        wanderAround.seekWeight = 0.4f;

        arrive.slowDownRadius = blackboard.pickupRadius * 2;
        arrive.closeEnoughRadius = blackboard.pickupRadius / 2;

        wanderAround.enabled = false;
        arrive.enabled = false;
    }

    public override void ReEnter()
    {
        base.ReEnter();

        ChangeState(State.INITIAL);
    }

    public override void Exit()
    {
        base.Exit();

        if (blackboard.resourcesFound)
        {
            blackboard.resourcesFound.transform.SetParent(null);
            blackboard.resourcesFound.tag = blackboard.resourcesTag;
            blackboard.resourcesFound = null;
        }

        wanderAround.enabled = false;
        arrive.enabled = false;
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
                While_Wander();
                break;
            case State.GOTO_RESOURCES:
                While_Goto_Resources();
                break;
            case State.GOTO_HIDEOUT:
                While_Goto_Hideout();
                break;
            case State.WAIT:
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0)
                    ChangeState(State.INITIAL);
                break;
            default:
                break;
        }
    }

    void ChangeState(State newState)
    {
        switch (currentState)
        {
            case State.INITIAL:
                break;
            case State.WANDER:
                wanderAround.enabled = false;
                break;
            case State.GOTO_RESOURCES:
                arrive.target = null;
                arrive.enabled = false;
                break;
            case State.GOTO_HIDEOUT:
                arrive.target = null;
                arrive.enabled = false;
                break;
            case State.WAIT:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case State.INITIAL:
                break;
            case State.WANDER:
                wanderAround.enabled = true;
                break;
            case State.GOTO_RESOURCES:
                arrive.target = blackboard.resourcesFound;
                arrive.enabled = true;
                break;
            case State.GOTO_HIDEOUT:
                arrive.target = blackboard.hideout;
                arrive.enabled = true;
                break;
            case State.WAIT:
                waitTimer = blackboard.waitDuration;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void While_Wander()
    {
        blackboard.resourcesFound = SensingUtils.FindInstanceWithinRadius(this.gameObject, blackboard.resourcesTag, blackboard.resourcesSensingRadius);

        if (blackboard.resourcesFound)
            ChangeState(State.GOTO_RESOURCES);
    }

    private void While_Goto_Resources()
    {
        if (blackboard.resourcesFound == null || blackboard.resourcesFound.Equals(null))
            ChangeState(State.WANDER);

        // If new resources appear closer to the ones we are going to, change target
        GameObject tempResources = SensingUtils.FindInstanceWithinRadius(this.gameObject, blackboard.resourcesTag, blackboard.resourcesSensingRadius);
        if (tempResources && tempResources != blackboard.resourcesFound)
        {
            blackboard.resourcesFound = tempResources;
            arrive.target = blackboard.resourcesFound;
        }

        // If we reached the resources, pick them up
        if (SensingUtils.DistanceToTarget(this.gameObject, blackboard.resourcesFound.gameObject) <= blackboard.pickupRadius)
        {
            blackboard.resourcesFound.tag = "Untagged";
            blackboard.resourcesFound.transform.SetParent(this.transform);
            ChangeState(State.GOTO_HIDEOUT);
        }
    }

    private void While_Goto_Hideout()
    {
        if (SensingUtils.DistanceToTarget(this.gameObject, blackboard.hideout) <= blackboard.hideoutRadius)
        {
            blackboard.resourcesFound.transform.SetParent(null);
            blackboard.resourcesFound = null;
            ChangeState(State.WAIT);
        }
    }
}
