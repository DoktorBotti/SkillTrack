using UnityEngine;
using System.Collections;

public class NonNormalWaypoint : Waypoint
{
    public RacetrackController raceContraoller;
    protected override void Start()
    {
        controller = raceContraoller;
        base.Start();
    }
}
