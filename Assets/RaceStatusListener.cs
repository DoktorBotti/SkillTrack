using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStatusListener : MonoBehaviour
{

    public RacetrackController controller;
    public GameObject lookAtObject;
    private TextMesh text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(lookAtObject.transform);
        transform.Rotate(Vector3.up, 180f);

        text.text = getStatusString();
    }

    public string getStatusString()
    {
        string res = "";
        Tuple<int, int> checkpointStatus = controller.CheckpointStatus;
        int checkedPoints = checkpointStatus.Item1;
        int totalPoints = checkpointStatus.Item2;
        switch (controller.raceStatus)
        {
            case RaceStatus.NOT_STARTED:
                res += "READY";
                break;
            case RaceStatus.FINISHED:
                res += $"FINISHED! \nTime: {controller.RaceDuration}";
                break;
            case RaceStatus.IN_PROGRESS:

                res += $"{controller.RaceDuration} \n Checkpoints {checkedPoints} / {totalPoints}";
                break;
            case RaceStatus.FAILED:
                res += $"RACE FAILED! \nTime: {controller.RaceDuration} \nCheckpoints: {checkedPoints} / {totalPoints}";
                break;

        }
        return res;
    }
}
