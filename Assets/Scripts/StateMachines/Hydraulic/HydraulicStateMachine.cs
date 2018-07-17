using UnityEngine;
using System.Collections;

public class HydraulicStateMachine : StateMachine
{
    // Use this for initialization
    public override void Start()
    {
        spawnPosition = transform.localPosition;
        System.Threading.Tasks.Task.Delay(startDelay).ContinueWith(t => currState = new UpHydraulicState(this));

    }
}
