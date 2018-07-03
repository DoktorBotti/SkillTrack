using UnityEngine;

public class StomperStateMachine : MonoBehaviour
{
    public double ShakeDistance;
    public double ShakeFrequency;

    public Transform UpPoition;
    public Transform DownPosition;

    [Tooltip("Time in msec to move from Up to down position")]
    public double TimeToMoveDown;
    [Tooltip("Time in msce to move from Down to Up position")]
    public double TimeToMoveUp;

    public double TimeSteady;
    public double TimeGrumble;

    private State currState;

    public StomperStateMachine()
    {
        currState = new SteadyStomperState(this);
    }

    public void ToNextState(State newState)
    {
        currState = newState;
    }

    public void Update()
    {
        currState.Execute();
    }
}