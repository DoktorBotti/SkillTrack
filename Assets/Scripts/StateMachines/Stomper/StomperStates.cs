using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


public abstract class StomperState : State
{
    protected StateMachine stateMachine;
    protected bool SwitchWasTriggered = false;

    public StomperState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Execute()
    {
        base.Execute();
        if (!SwitchWasTriggered)
        {
            SwitchWasTriggered = true;
            TriggerStateChange();
        }
    }

    public abstract void TriggerStateChange();
}

public class UpStomperState : SteadyState
{
    public UpStomperState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        return new GrumbleStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeSteady).ContinueWith(t => stateMachine.ToNextState(getNextState));
    }
}

public class GrumbleStomperState : GrumbleState
{
    public GrumbleStomperState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.spawnPosition + stateMachine.UpPoition;
        return new StompStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeGrumble).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class StompStomperState : GoDownState
{
    public StompStomperState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.DownPosition + stateMachine.spawnPosition;

        return new DownStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveDown).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class DownStomperState : SteadyState
{
    public DownStomperState(StateMachine stomper) : base(stomper)
    {

    }
    public override State getNextState()
    {
        return new GoUpStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.DownTime).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class GoUpStomperState : GoUpState
{

    public GoUpStomperState(StateMachine stomper) : base(stomper)
    {

    }
 
    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.UpPoition + stateMachine.spawnPosition;
        return new UpStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveUp).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}