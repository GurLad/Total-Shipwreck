using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int LevelNum = -1;
    private void Start()
    {
        if (LevelNum != -1)
        {
            if (LevelNum > SavedData.Load<int>("LevelNum", 0))
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
    public void Click()
    {
        if (LevelNum <= -1)
        {
            CurrentLevel.LoadLevel();
        }
        else
        {
            CurrentLevel.LoadLevel(LevelNum);
        }
    }
}
