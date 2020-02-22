using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(FSM_HEN_EATS))]
[RequireComponent(typeof(Seek))]
[RequireComponent(typeof(HEN_BLACKBOARD))]
public class FSM_HEN_EATS_ALONE : FiniteStateMachine
{
    public enum State { INITIAL, CALM, ANGRY };
    public State currentState;

    /* COMPLETE */

    
    void Start()
    {
       /* COMPLETE */
    }


    // Update is called once per frame
    void Update()
    {
        /* COMPLETE */
    }

    void ChangeState(State newState)
    {

        /* COMPLETE */
    }

}
