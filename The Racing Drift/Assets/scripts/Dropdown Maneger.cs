using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownManeger : MonoBehaviour
{
    public MenuManeger Menu;
    public int ID;

    public void getID(int val)
    {
        ID = val;
        Menu.getCar();
        Menu.getTrack();
    }
}
