using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackManeger : MonoBehaviour
{
    public MenuManeger menu;
    public MenuData menuData;
    public SceneManeger Scene;

    public GameObject GTR;
    public GameObject GTR720;
    public GameObject Hoonicorn;
    public Transform parent;

    public CheckpointManager Finish;
    public CheckpointManager splitChekpoint;
    public CheckpointManager antiCheatChekpoint1;
    public CheckpointManager antiCheatChekpoint2;
    private int NumLap = 2;
    //public bool finishcol = false;
    private bool allcheckpoints = false;
    public int CarID;
    


    public void Start()
    {
        menu.Resume();
        CarID = menuData.CarID;
        loadCar();
    }
    public void loadCar()
    {
        //GameObject child;
        if (CarID == 0)
        {
            Instantiate(GTR, parent);
        }
        else if (CarID == 1)
        {
            Instantiate(GTR720, parent);
        }
        else if(CarID == 2)
        {
            Instantiate(Hoonicorn, parent);
        }
    }
    public void PauseResume()
    {
        if (Time.timeScale == 1)
        {
            PauseGame();
            menu.Pause();
        }
        else if (Time.timeScale == 0)
        {
            ResumeGame();
            menu.Resume();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void finishline()
    {
        if (menuData.TrackID == 0)
        {
            if(splitChekpoint.Checkpoint == true)
            {
                allcheckpoints = true;
            }
        }
        if (menuData.TrackID == 1)
        {
            if ((splitChekpoint.Checkpoint && antiCheatChekpoint1) == true)
            {
                allcheckpoints = true;
            }
        }
        if (menuData.TrackID == 2)
        {
            if ((splitChekpoint.Checkpoint && antiCheatChekpoint1 && antiCheatChekpoint2) == true)
            {
                allcheckpoints = true;
            }
        }
        if (allcheckpoints == true && Finish.finish == true)
        {
            Finish.lap++;
            if (Finish.lap == NumLap)
            {
                Scene.nextRace();
            }
            allcheckpoints = false;
            splitChekpoint.Checkpoint = false;
            if (menuData.TrackID > 0)
            {
                antiCheatChekpoint1.Checkpoint = false;
                if (menuData.TrackID > 0)
                {
                    antiCheatChekpoint2.Checkpoint = false;
                }
            }
        }
    }
}
