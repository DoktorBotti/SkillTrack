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

public abstract class SteadyState : StomperState
{
    public SteadyState(StateMachine stomper) : base(stomper)
    {
    }

    public override void Execute()
    {
        base.Execute();
    }
}

public abstract class GrumbleState : StomperState
{
    float phaseShift = Mathf.PI / 2;
    float passedTime;

    public GrumbleState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Execute()
    {
        base.Execute();
        passedTime += Time.deltaTime * 1000;
        passedTime %= (float)(stateMachine.ShakeFrequency * 2 * Mathf.PI);

        Vector3 toShake = new Vector3();
        toShake.x = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency) - 0f) * (float)stateMachine.ShakeDistance;
        toShake.y = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency + phaseShift) - 0f) * (float)stateMachine.ShakeDistance;
        toShake.z = (Mathf.Sin(passedTime * (float)stateMachine.ShakeFrequency + 2 * phaseShift) - 0f) * (float)stateMachine.ShakeDistance;
        stateMachine.transform.localPosition += toShake;
    }
}

public abstract class GoDownState : StomperState
{
    private float downVelocity;
    private Vector3 travelVec, startPos;

    private float passedTime = 0;
    public GoDownState(StateMachine stomper) : base(stomper)
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
}

public abstract class GoUpState : StomperState
{
    private float upVelocity;
    private Vector3 travelVec, startPosition;
    private float passedTime = 0;

    public GoUpState(StateMachine stomper) : base(stomper)
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
}