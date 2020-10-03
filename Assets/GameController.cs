using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Stats")]
    public float WaterRiseRate;
    public float MaxWaterValue;
    [Header("Objects")]
    public MoveWater WaterObject;
    private float WaterValue;
    private void Update()
    {
        WaterValue += Time.deltaTime * WaterRiseRate;
        if (WaterValue >= MaxWaterValue)
        {
            SceneManager.LoadScene(Application.loadedLevel);
        }
        WaterObject.HeightMod = WaterValue;
    }
}
