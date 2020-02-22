using UnityEngine;
using FSM;
using Steerings;

[RequireComponent(typeof(WanderAround))]
[RequireComponent(typeof(Flee))]
public class FSM_CHICK : FiniteStateMachine
{
    public enum State { INITIAL, WANDER, FLEE };
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Flee flee;
    private KinematicState ks;
    private HEN_BLACKBOARD blackboard;
    private GameObject theHen;

    // Start is called before the first frame update
    void Start()
    {
        wanderAround = GetComponent<WanderAround>();
        flee = GetComponent<Flee>();
        ks = GetComponent<KinematicState>();
        theHen = GameObject.Find("HEN");
        blackboard = theHen.GetComponent<HEN_BLACKBOARD>();

        wanderAround.attractor = theHen;
        wanderAround.seekWeight = 0.4f;
        flee.target = theHen;
        wanderAround.enabled = false;
        flee.enabled = false;
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
                if (SensingUtils.DistanceToTarget(gameObject, theHen)<=blackboard.chickDetectionRadius/3)
                {
                    ChangeState(State.FLEE);
                    break;
                }
                break;
            case State.FLEE:
                if (SensingUtils.DistanceToTarget(gameObject, theHen) >= blackboard.chickFarEnoughRadius*1.5)
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
                wanderAround.enabled = false;
                break;
            case State.FLEE:
                ks.maxAcceleration /= 3;
                ks.maxSpeed /= 7;
                flee.enabled = false;
                break;
        }

        switch (newState)
        {
            case State.INITIAL: break;
            case State.WANDER:
                wanderAround.enabled = true;
                break;
            case State.FLEE:
                GetComponent<AudioSource>().Play();
                ks.maxAcceleration *= 3;
                ks.maxSpeed *= 7;
                flee.enabled = true;
                break;
        }
        currentState = newState;
    }

}
