using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManeger : MonoBehaviour
{
    public SceneManeger Scene;
    public TrackManeger TrackManeger;
    public DropdownManeger dropdownTrack;
    public DropdownManeger dropdownCar;

    public MenuData menuData;

    public int TrackID;

    public GameObject GTR;
    public GameObject GTR720;
    public GameObject Hoonicorn;
    private int CarID;

    private int Power = 0;
    private int Stearing;
    private int Braking;
    private int Boost;
    private int Drift;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI stearingText;
    public TextMeshProUGUI brakingText;
    public TextMeshProUGUI boostText;
    public TextMeshProUGUI driftText;
    

    public Canvas canvas;
    private void Start()
    {
        TrackID = menuData.TrackID;
        if (TrackID == 10)
        {
            getCar();
            getTrack();
            LoadCarStats();
        }
    }
    public void getTrack()
    {
        TrackID = dropdownTrack.ID;
    }
    public void getCar()
    {
        CarID = dropdownCar.ID;
        LoadCarStats();
    }

    private void LoadCarStats()
    {
        if (menuData.TrackID == 10)
        {
            if (CarID == 0)
            {
                Power = GTR.GetComponent<CarStats>().Power;
                Stearing = GTR.GetComponent<CarStats>().Stearing;
                Braking = GTR.GetComponent<CarStats>().Braking;
                Boost = GTR.GetComponent<CarStats>().Boost;
                Drift = GTR.GetComponent<CarStats>().Drift;
            }
            else if (CarID == 1)
            {
                Power = GTR720.GetComponent<CarStats>().Power;
                Stearing = GTR720.GetComponent<CarStats>().Stearing;
                Braking = GTR720.GetComponent<CarStats>().Braking;
                Boost = GTR720.GetComponent<CarStats>().Boost;
                Drift = GTR720.GetComponent<CarStats>().Drift;
            }
            else if (CarID == 2)
            {
                Power = Hoonicorn.GetComponent<CarStats>().Power;
                Stearing = Hoonicorn.GetComponent<CarStats>().Stearing;
                Braking = Hoonicorn.GetComponent<CarStats>().Braking;
                Boost = Hoonicorn.GetComponent<CarStats>().Boost;
                Drift = Hoonicorn.GetComponent<CarStats>().Drift;
            }
            powerText.text = Power.ToString();
            brakingText.text = Braking.ToString();
            stearingText.text = Stearing.ToString();
            boostText.text = Boost.ToString();
            driftText.text = Drift.ToString();
        }
    }

    public void StartRace()
    {
        menuData.TrackID = TrackID;
        Scene.LoadScene();
        menuData.CarID = CarID;
    }
    public void quitGame()
    {
        Application.Quit();
    }


    public void ResumeBtn()
    {
        TrackManeger.ResumeGame();
        Resume();
    }
    public void RestartBtn()
    {
        Scene.LoadScene();
        TrackManeger.ResumeGame();
    }
    public void ExitBtn()
    {
        Scene.ToMainmenu();
        TrackManeger.ResumeGame();
    }

    public void Pause()
    {
        canvas = FindObjectOfType<Canvas>();
        canvas.enabled = true;
    }
    public void Resume()
    {
        canvas = FindObjectOfType<Canvas>();
        canvas.enabled = false;
    }
}
