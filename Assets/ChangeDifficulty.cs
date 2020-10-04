using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDifficulty : MonoBehaviour
{
    public Text Text;
    private void Start()
    {
        Text.text = SavedData.Load<int>("Difficult") == 1 ? "Hard" : "Normal";
    }
    public void Click()
    {
        if (SavedData.Load<int>("Difficult") == 1)
        {
            SavedData.Save("Difficult", 0);
            Text.text = "Normal";
        }
        else
        {
            SavedData.Save("Difficult", 1);
            Text.text = "Hard";
        }
    }
}
