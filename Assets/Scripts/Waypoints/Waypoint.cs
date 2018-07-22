using System;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    protected RacetrackController controller;
    private RacetrackController.WaypointTriggeredDelegate checkedDelegate;
    private ChangeColorWaypoint colorScript;
    private bool _isChecked;
    public bool isChecked
    {
        get
        {
            return _isChecked;
        }
    }

    protected virtual void Start()
    {
        if (controller == null)
        {
            controller = GetComponentInParent<RacetrackController>();
        }

        colorScript = GetComponent<ChangeColorWaypoint>();
        if (controller == null)
        {
            Debug.LogError($"Racetrack controller not found! - {gameObject.name}");
        }

        checkedDelegate = controller.RegisterWaypoint(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && !isChecked)
        {
            _isChecked = checkedDelegate(this);
            if (isChecked == true && colorScript != null)
            {
                colorScript.applyColor();
            }
        }


    }

    internal void reset()
    {
        _isChecked = false;
        if (colorScript != null)
        {
            colorScript.resetColor();

        }
    }

}