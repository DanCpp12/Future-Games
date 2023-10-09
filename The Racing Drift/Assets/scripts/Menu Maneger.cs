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

    public GameObject car1;
    public GameObject car2;
    public GameObject hoonicorn;
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
        LoadCarStats();
        TrackID = menuData.TrackID;
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
                Power = car1.GetComponent<CarStats>().Power;
                Stearing = car1.GetComponent<CarStats>().Stearing;
                Braking = car1.GetComponent<CarStats>().Braking;
                Boost = car1.GetComponent<CarStats>().Boost;
                Drift = car1.GetComponent<CarStats>().Drift;
            }
            else if (CarID == 1)
            {
                Power = car2.GetComponent<CarStats>().Power;
                Stearing = car2.GetComponent<CarStats>().Stearing;
                Braking = car2.GetComponent<CarStats>().Braking;
                Boost = car2.GetComponent<CarStats>().Boost;
                Drift = car2.GetComponent<CarStats>().Drift;
            }
            else if (CarID == 2)
            {
                Power = hoonicorn.GetComponent<CarStats>().Power;
                Stearing = hoonicorn.GetComponent<CarStats>().Stearing;
                Braking = hoonicorn.GetComponent<CarStats>().Braking;
                Boost = hoonicorn.GetComponent<CarStats>().Boost;
                Drift = hoonicorn.GetComponent<CarStats>().Drift;
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
