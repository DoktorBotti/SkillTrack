using System;
using System.Collections;

public abstract class State
{

    public abstract void Execute();
    public abstract State getNextState();

}

public class SteadyStomperState : State
{
    private bool SwitchWasTriggered = false;

    private StomperStateMachine stateMachine;

    public SteadyStomperState(StomperStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Execute()
    {
        if (!SwitchWasTriggered)
        {
            delayedStateChangeRequest();
        }
    }

    public override State getNextState()
    {
        return new GrumbleStomperState(stateMachine);
    }
}

public class GrumbleStomperState : State
{
    private StomperStateMachine stateMachine;
    private DateTime lastStep;

    public GrumbleStomperState(StomperStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Execute()
    {
        
    }
}