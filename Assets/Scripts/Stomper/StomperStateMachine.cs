using UnityEngine;

public class StomperStateMachine : MonoBehaviour
{
    public delegate State GetStateDelegate();
    public double ShakeDistance = 2;
    public double ShakeFrequency = 30;

    [Tooltip("local space deviation")]
    public Vector3 UpPoition;
    [Tooltip("local space deviation")]
    public Vector3 DownPosition;

    [Tooltip("Time in msec to move from Up to down position")]
    public int TimeToMoveDown = 400;
    [Tooltip("Time in msce to move from Down to Up position")]
    public int TimeToMoveUp = 600;

    /// <summary>
    /// time im ms
    /// </summary>
    public int TimeSteady = 2000;
    /// <summary>
    /// time in ms
    /// </summary>
    public int TimeGrumble = 500;

    /// <summary>
    /// time in ms
    /// </summary>
    public int DownTime = 200;

    private State currState;
    private GetStateDelegate nextState;

    public StomperStateMachine()
    {
        currState = new SteadyStomperState(this);
    }

    public void ToNextState(GetStateDelegate deleg)
    {
        nextState = deleg;
    }

    public void Update()
    {
        if(nextState != null)
        {
            if(name == "FirstStomper")
            {
                Debug.Log("Changing Status");
            }
            currState = nextState();

            nextState = null;
        }
        currState.Execute();
    }

    /*
     * public void FixedUpdate()
     * {
     * currState.ExecuteFixed();
     * }
     */
}