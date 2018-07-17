

using System.Threading.Tasks;

public class UpHydraulicState : SteadyState
{
    public UpHydraulicState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        return new GoDownHydraulicState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeSteady).ContinueWith(t => stateMachine.ToNextState(getNextState));
    }
}

public class GoDownHydraulicState : GoDownState
{
    public GoDownHydraulicState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.DownPosition + stateMachine.spawnPosition;

        return new DownHydraulicState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveDown).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class DownHydraulicState : SteadyState
{
    public DownHydraulicState(StateMachine stomper) : base(stomper)
    {

    }
    public override State getNextState()
    {
        return new GoUpHydraulicState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.DownTime).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}

public class GoUpHydraulicState : GoUpState
{

    public GoUpHydraulicState(StateMachine stomper) : base(stomper)
    {

    }

    public override State getNextState()
    {
        stateMachine.transform.localPosition = stateMachine.UpPoition + stateMachine.spawnPosition;
        return new UpHydraulicState(stateMachine);
    }

    public override void TriggerStateChange()
    {
        Task.Delay(base.stateMachine.TimeToMoveUp).ContinueWith(t => stateMachine.ToNextState(getNextState));

    }
}