using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacetrackController : MonoBehaviour
{

    public delegate bool WaypointTriggeredDelegate(Waypoint waypoint);
    private List<Waypoint> waypoints;
    private int CheckedWaypoints;
    private float startTime, endTime;

    public float RaceDuration
    {
        get
        {
            if (_raceStatus == RaceStatus.IN_PROGRESS)
            {
                return Time.time - startTime;
            }
            else if (_raceStatus == RaceStatus.NOT_STARTED)
            {
                return 0;
            }
            else
            {
                return endTime;
            }
        }
    }

    public Tuple<int, int> CheckpointStatus
    {
        get
        {
            return new Tuple<int, int>(CheckedWaypoints, waypoints.Count);
        }
    }

    private RaceStatus _raceStatus;

    public RaceStatus raceStatus
    {
        get
        {
            return _raceStatus;
        }
    }

    internal void StartRace()
    {
        ResetRace();
        Debug.Log("Race Started");
        startTime = Time.time;
        _raceStatus = RaceStatus.IN_PROGRESS;
    }

    // Use this for initialization
    void Start()
    {
        waypoints = new List<Waypoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal WaypointTriggeredDelegate RegisterWaypoint(Waypoint waypoint)
    {
        if (waypoint == null)
        {
            return null;
        }

        waypoints.Add(waypoint);
        return UpdateWaypointState;
    }

    private bool UpdateWaypointState(Waypoint waypoint)
    {
        if (_raceStatus == RaceStatus.IN_PROGRESS)
        {
            CheckedWaypoints++;
        }

        if (waypoints.Count <= CheckedWaypoints)
        {
            // race finished
            endRace();
        }
        return raceStatus == RaceStatus.IN_PROGRESS;
    }

    private void endRace()
    {
        endTime = Time.time - startTime;
        _raceStatus = RaceStatus.FINISHED;

        Debug.Log($"Race finished! Time: {endTime}");
    }

    public  void FailRace()
    {
        if(raceStatus == RaceStatus.FAILED)
        {
            return;
        }
        endTime = Time.time - startTime;
        _raceStatus = RaceStatus.FAILED;
        Debug.Log($"Race failed! Time: {endTime} , Checked Waypoints: {CheckedWaypoints}");
    }

    public void ResetRace()
    {
        CheckedWaypoints = 0;
        _raceStatus = RaceStatus.NOT_STARTED;
        foreach (Waypoint w in waypoints)
        {
            w.reset();
        }
    }
}
