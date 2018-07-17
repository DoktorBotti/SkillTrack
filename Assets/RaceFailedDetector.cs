using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFailedDetector : MonoBehaviour {
    public RacetrackController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FatalObject")
        {
            controller.FailRace();
        }
    }
}
