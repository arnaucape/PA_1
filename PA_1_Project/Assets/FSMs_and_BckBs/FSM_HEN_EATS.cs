using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderAround))]
[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(HEN_BLACKBOARD))]
[RequireComponent(typeof(AudioSource))]
public class FSM_HEN_EATS : FiniteStateMachine
{
    public enum State {INITIAL, WANDER, GOTO_WORM, EAT};
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Arrive arrive;
    private HEN_BLACKBOARD blackboard;
    private AudioSource audioSource;

    private GameObject worm;
    private float elapsedTime;

    void Start()
    {
        blackboard = GetComponent<HEN_BLACKBOARD>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();

        wanderAround.seekWeight = 0.0f;
        wanderAround.attractor = blackboard.attractor;

        wanderAround.enabled = false;
        arrive.enabled = false;
    }

    public override void Exit()
    {
        /* COMPLETE */
    }

    public override void ReEnter()
    {
       /* COMPLETE */
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
        }
    }

    void ChangeState (State newState)
    {

        /* COMPLETE */
    }
}
