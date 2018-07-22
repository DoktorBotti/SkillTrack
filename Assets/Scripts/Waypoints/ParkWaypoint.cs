using UnityEngine;
using System.Collections;

/// <summary>
/// ASSERTIONS:
/// - there is only one Car GameObject tagged
///   - car has a BoundBox component !that stays the same!
/// </summary>
public class ParkWaypoint : Waypoint
{
    public RacetrackController raceContraoller;

    [Tooltip("Tolerance in degrees, in which the orientation gets accepted")]
    public float OrientationTolerance = 5;

    [Tooltip("Maximum speed (m/s) , when \'parking\' is accepted")]
    public float maxSpeed;

    private Bounds ownBound;
    private BoundBox carBoundBox;

    protected override void Start()
    {
        controller = raceContraoller;
        base.Start();

        ownBound = GetComponent<BoxCollider>().bounds;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "Car")
        {
            return;
        }

        
    }
}

