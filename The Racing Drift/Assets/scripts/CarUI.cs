using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarUI : MonoBehaviour
{
    public carController Car;
    public TextMeshProUGUI kph;
    public TextMeshProUGUI gear;

    private void FixedUpdate()
    {
        kph.text = "" + (int)Car.KPH; ;
        gear.text = "" + (Car.gearNum + 1);
    }
}
