using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CurrentLevel
{
    private const int NumLevels = 3;
    public static int Value { get; private set; }
    public static void FinishLevel()
    {
        Value++;
        if (Value > SavedData.Load<int>("LevelNum"))
        {
            SavedData.Save<int>("LevelNum", Value);
        }
        if (Value < NumLevels)
        {
            SceneManager.LoadScene("NextLevel");
        }
        else
        {
            SceneManager.LoadScene("Win");
        }
    }
    public static void LoadLevel()
    {
        SceneManager.LoadScene("Level" + Value);
    }
    public static void LoadLevel(int value)
    {
        Value = value;
        SceneManager.LoadScene("Level" + value);
    }
}
