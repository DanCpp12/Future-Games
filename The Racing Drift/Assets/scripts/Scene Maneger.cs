using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger: MonoBehaviour
{
    public MenuData menuData;
    public int SceneID = 0;

    public void ToMainmenu()
    {
        SceneID = 10;
        menuData.TrackID = SceneID;
        LoadScene();
    }

    public void LoadScene()
    {
        SceneID = menuData.TrackID;
        if (SceneID == 0)
        {
            SceneManager.LoadSceneAsync("Circuit of Kings", LoadSceneMode.Single);
        }
        else if (SceneID == 1)
        {
            SceneManager.LoadSceneAsync("The Snake Raceway", LoadSceneMode.Single);
        }
        else if (SceneID == 2)
        {
            SceneManager.LoadSceneAsync("Bernese Alps", LoadSceneMode.Single);
        }
        else if(SceneID == 10)
        {
            SceneManager.LoadSceneAsync("menu", LoadSceneMode.Single);
        }
    }
    public void nextRace()
    {
        if (SceneID == 2)
        {
            SceneID = 10;
        }
        else
        {
            SceneID++;
        }
        menuData.TrackID = SceneID;
        LoadScene();
        Debug.Log("Enterd nextRace()");
    }
}
