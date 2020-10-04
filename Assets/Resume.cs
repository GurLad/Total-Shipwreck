using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public GameObject PauseObject;
    public void Click()
    {
        PauseObject.SetActive(false);
        Time.timeScale = 1;
    }
}
