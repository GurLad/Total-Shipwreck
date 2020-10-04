using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerByNum : MonoBehaviour
{
    public int Num;
    public PirateController Pirate;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0 + Num))
        {
            GameController.Current.SetPlayer(Pirate);
        }
    }
}
