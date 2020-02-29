using UnityEngine;
using FSM;
using Steerings;

[RequireComponent(typeof(FSM_GatherResources), typeof(RepulsionTowards))]
public class FSM_SurvivorPanic : FiniteStateMachine
{
    public enum State { INITIAL, GATHER, FLEE, WAITING_INSIDE_HIDEOUT};
    public State currentState = State.INITIAL;

    private FSM_GatherResources gatherResources_FSM;
    private RepulsionTowards flee;

    private Survivor_Blackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        gatherResources_FSM = GetComponent<FSM_GatherResources>();
        flee = GetComponent<RepulsionTowards>();
        blackboard = GetComponent<Survivor_Blackboard>();

        flee.idTag = blackboard.zombieTag;
        flee.repulsionThreshold = blackboard.zombieFarEnough;
        flee.target = blackboard.hideout;
        flee.repulsionForce = blackboard.repulsionForce;

        gatherResources_FSM.enabled = false;
        flee.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.GATHER);
                break;
            case State.GATHER:
                While_Gather();
                break;
            case State.FLEE:
                While_Flee();
                break;
            case State.WAITING_INSIDE_HIDEOUT:
                While_WaitingInHideout();
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
            case State.GATHER:
                gatherResources_FSM.Exit();
                gatherResources_FSM.enabled = false;
                break;
            case State.FLEE:
                flee.enabled = false;
                break;
            case State.WAITING_INSIDE_HIDEOUT:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case State.INITIAL:
                break;
            case State.GATHER:
                gatherResources_FSM.ReEnter();
                gatherResources_FSM.enabled = true;
                break;
            case State.FLEE:
                flee.enabled = true;
                break;
            case State.WAITING_INSIDE_HIDEOUT:
                blackboard.insideHideout = true;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void While_Gather()
    {
        blackboard.zombieDetected = SensingUtils.FindInstanceWithinRadius(this.gameObject, blackboard.zombieTag, blackboard.zombieCloseEnough);
        blackboard.insideHideout = SensingUtils.DistanceToTarget(this.gameObject, blackboard.hideout) <= blackboard.hideoutRadius;

        if (blackboard.zombieDetected && !blackboard.insideHideout)
            ChangeState(State.FLEE);
    }

    private void While_Flee()
    {
        if (SensingUtils.DistanceToTarget(this.gameObject, blackboard.hideout) <= blackboard.hideoutRadius)
            ChangeState(State.WAITING_INSIDE_HIDEOUT);

        // If we flee into a another zombie, flee from that one
        GameObject tempZombie = SensingUtils.FindInstanceWithinRadius(this.gameObject, blackboard.zombieTag, blackboard.zombieCloseEnough);
        if (tempZombie && tempZombie != blackboard.zombieDetected)
        {
            blackboard.zombieDetected = tempZombie;
        }

        if (SensingUtils.DistanceToTarget(this.gameObject, blackboard.zombieDetected) >= blackboard.zombieFarEnough)
            ChangeState(State.GATHER);
    }

    private void While_WaitingInHideout()
    {
        blackboard.zombieDetected = SensingUtils.FindInstanceWithinRadius(this.gameObject, blackboard.zombieTag, blackboard.zombieCloseEnough);

        if (!blackboard.zombieDetected)
            ChangeState(State.GATHER);

    }
}
