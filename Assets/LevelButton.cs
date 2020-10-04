using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int LevelNum = -1;
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
