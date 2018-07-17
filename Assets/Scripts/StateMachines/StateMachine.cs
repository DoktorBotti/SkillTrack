using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public delegate State GetStateDelegate();
    public double ShakeDistance = 0.1;  
    public double ShakeFrequency = 60;

    [Tooltip("local space deviation")]
    public Vector3 UpPoition;
    [Tooltip("local space deviation")]
    public Vector3 DownPosition;

    [Tooltip("Time in msec to move from Up to down position")]
    public int TimeToMoveDown = 300;
    [Tooltip("Time in msce to move from Down to Up position")]
    public int TimeToMoveUp = 600;

    /// <summary>
    /// time im ms
    /// </summary>
    public int TimeSteady = 2500;
    /// <summary>
    /// time in ms
    /// </summary>
    public int TimeGrumble = 500;

    /// <summary>
    /// time in ms
    /// </summary>
    public int DownTime = 200;

    protected State currState;
    private GetStateDelegate nextState;

    [HideInInspector]
    public Vector3 spawnPosition;

    [Tooltip("Time in ms before the stomper gets active")]
    public int startDelay = 0;

    public abstract void Start();

    public void ToNextState(GetStateDelegate deleg)
    {
        nextState = deleg;
    }

    public void Update()
    {
        if (nextState != null)
        {
            currState = nextState();

            nextState = null;
        }
        if (currState != null)
        {


            currState.Execute();
        }
    }

    /*
     * public void FixedUpdate()
     * {
     * currState.ExecuteFixed();
     * }
     */
}