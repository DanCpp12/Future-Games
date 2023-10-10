using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class CheckpointManager : MonoBehaviour
{
    public GameObject hitbox;
    public TrackManeger trackManeger;
    public bool finish;
    public bool Checkpoint;
    public int lap = 0;

    private void OnTriggerEnter(Collider other)
    {
        finish = true;
        Checkpoint = true;
        trackManeger.finishline();
    }
    private void OnTriggerExit(Collider other)
    {
        finish = false;
    }
    public bool Finish(bool checkpoints)
    {
        if (checkpoints == true) 
        {
            Checkpoint = false;
            return true;
        }
        return false;
    }
}
