using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRaceTrigger : MonoBehaviour {

    public RacetrackController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && controller.raceStatus != RaceStatus.IN_PROGRESS)
        {
            controller.StartRace();
        }


    }
}
