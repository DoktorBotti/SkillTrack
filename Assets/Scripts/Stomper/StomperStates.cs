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
    float passedTime;

    public GrumbleStomperState(StomperStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Execute()
    {
        base.Execute();
        passedTime += Time.deltaTime * 1000;
        passedTime %= (float) (stateMachine.ShakeFrequency * 2 * Mathf.PI);

        Vector3 toShake = new Vector3();
        toShake.x = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency) - 0f) * (float) stateMachine.ShakeDistance;
        toShake.y = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency + phaseShift) - 0f) * (float)stateMachine.ShakeDistance;
        toShake.z = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency + 2 * phaseShift) - 0f) * (float) stateMachine.ShakeDistance;
        stateMachine.transform.localPosition += toShake;
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

public class StompStomperState : StomperState
{
    private float downVelocity;
    private Vector3 travelVec, startPos;

    private float passedTime = 0;
    public StompStomperState(StomperStateMachine stomper) : base(stomper)
    {
        travelVec = stateMachine.DownPosition - stateMachine.UpPoition;
        startPos = stateMachine.spawnPosition + stateMachine.UpPoition;

        downVelocity = travelVec.magnitude / stateMachine.TimeToMoveDown;
    }

    public override void Execute()
    {
        base.Execute();

        passedTime += Time.deltaTime * 1000;
        Vector3 newPos = startPos + downVelocity * travelVec.normalized * passedTime;
        stateMachine.transform.localPosition = newPos;
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

public class DownStomperState : StomperState
{

    public DownStomperState(StomperStateMachine stomper) : base(stomper)
    {
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
    private Vector3 travelVec, startPosition;
    private float passedTime = 0;

    public GoUpStomperState(StomperStateMachine stomper) : base(stomper)
    {
        startPosition = stateMachine.spawnPosition + stateMachine.DownPosition;
        travelVec = stateMachine.UpPoition - stateMachine.DownPosition;
        upVelocity = travelVec.magnitude / stateMachine.TimeToMoveUp;
    }

    public override void Execute()
    {
        base.Execute();
        passedTime += Time.deltaTime * 1000;
        Vector3 newPos = startPosition + upVelocity * travelVec.normalized * passedTime;
        stateMachine.transform.localPosition = newPos;
    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.UpPoition + stateMachine.spawnPosition;
        return new SteadyStomperState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveUp).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}