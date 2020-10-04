using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Current;
    [Header("Stats")]
    public float WaterRiseRate;
    public float MaxWaterValue;
    [Header("Objects")]
    public MoveWater WaterObject;
    public PPTScript RecordingShader;
    public FollowObject Camera;
    [Header("UI")]
    public Image WaterIndicator;
    [HideInInspector]
    public float WaterValue;
    public float WaterHeight
    {
        get
        {
            return WaterObject.HeightMod + WaterValue;
        }
    }
    private float maxWaterIndicatorSize;
    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        maxWaterIndicatorSize = WaterIndicator.rectTransform.sizeDelta.x;
    }
    private void Update()
    {
        WaterValue += Time.deltaTime * WaterRiseRate;
        WaterIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * WaterValue / MaxWaterValue, WaterIndicator.rectTransform.sizeDelta.y);
        if (WaterValue >= MaxWaterValue)
        {
            SceneManager.LoadScene(Application.loadedLevel);
        }
        WaterObject.WaterHeight = WaterValue;
        int levelStage = Mathf.FloorToInt(3 * WaterValue / MaxWaterValue) + 1;
        CrossfadeMusicPlayer.Instance.Play("Level" + levelStage, true);
    }
    public void SetPlayer(PirateController pirate)
    {
        pirate.Active = true;
        Camera.Target = pirate.transform;
    }
    public void SetRecording(bool mode)
    {
        RecordingShader.enabled = mode;
    }
}
