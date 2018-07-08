using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public abstract class State
{
    public virtual void Execute()
    {

    }
    public virtual void ExecuteFixed()
    {

    }
    public abstract State getNextState();

}

public abstract class StomperState : State
{
    protected StomperStateMachine stateMachine;
    protected bool SwitchWasTriggered = false;

    public StomperState(StomperStateMachine stateMachine)
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

public class SteadyStomperState : StomperState
{
    public SteadyStomperState(StomperStateMachine stomper) : base(stomper)
    {
        Debug.Log("Steady");
    }

    public override void Execute()
    {
        base.Execute();
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

public class GrumbleStomperState : StomperState
{
    float phaseShift = Mathf.PI / 2;
    Vector3 origPosition;

    public GrumbleStomperState(StomperStateMachine stateMachine) : base(stateMachine)
    {
        Debug.Log("Grumble");
        origPosition = stateMachine.transform.localPosition;
    }

    public override void Execute()
    {
        base.Execute();

        Vector3 toShake = new Vector3();
        toShake.x = Mathf.Sin(Time.deltaTime * (float)stateMachine.ShakeFrequency) * (float) stateMachine.ShakeDistance - (float) stateMachine.ShakeDistance / 2;
        toShake.y = Mathf.Sin(Time.deltaTime * (float)stateMachine.ShakeFrequency + phaseShift) * (float) stateMachine.ShakeDistance- (float) stateMachine.ShakeDistance / 2;
        toShake.z = Mathf.Sin(Time.deltaTime * (float)stateMachine.ShakeFrequency + 2 * phaseShift) * (float) stateMachine.ShakeDistance- (float) stateMachine.ShakeDistance / 2;
        stateMachine.transform.localPosition += toShake;
    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = origPosition;
        return new StompStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeGrumble).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class StompStomperState : StomperState
{
    private float downVelocity;
    private Vector3 travelVec, rootPosition;
    private float passedTime = 0;
    public StompStomperState(StomperStateMachine stomper) : base(stomper)
    {
        Debug.Log("Stomp");
        travelVec = stateMachine.DownPosition - stateMachine.UpPoition;
        downVelocity = travelVec.magnitude / stateMachine.TimeToMoveDown;
        rootPosition = stateMachine.transform.localPosition;
    }

    public override void Execute()
    {
        base.Execute();

        passedTime += Time.deltaTime * 1000;
        stateMachine.transform.localPosition = stateMachine.UpPoition + downVelocity * passedTime * travelVec.normalized + rootPosition;
    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.DownPosition + rootPosition;

        return new DownStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveDown).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class DownStomperState : StomperState
{

    public DownStomperState(StomperStateMachine stomper) : base(stomper)
    {
        Debug.Log("IsDown");
    }

    public override void Execute()
    {
        base.Execute();
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

public class GoUpStomperState : StomperState
{
    private float upVelocity;
    private Vector3 travelVec, rootPosition;
    private float passedTime = 0;
    public GoUpStomperState(StomperStateMachine stomper) : base(stomper)
    {
        Debug.Log("Up");
        rootPosition = stateMachine.transform.localPosition;
        travelVec = stateMachine.UpPoition - stateMachine.DownPosition;
        upVelocity = travelVec.magnitude / stateMachine.TimeToMoveUp;
    }

    public override void Execute()
    {
        base.Execute();
        passedTime += Time.deltaTime * 1000;
        stateMachine.transform.localPosition = stateMachine.DownPosition + upVelocity * passedTime * travelVec.normalized + rootPosition;
    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.UpPoition + rootPosition;
        return new SteadyStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveUp).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}