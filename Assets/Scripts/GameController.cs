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
    public float SecondsTillRain;
    public int TreasureAmount;
    [Header("Objects")]
    public MoveWater WaterObject;
    public PPTScript RecordingShader;
    public FollowObject Camera;
    public List<GameObject> RainIndicators;
    [Header("UI")]
    public Image WaterIndicator;
    public Image HoleIndicator;
    public Text HoleCountIndicator;
    public Image RainIndicator;
    public Text RainCountIndicator;
    public Image TreasureIndicator;
    public Text TreasureCountIndicator;
    [HideInInspector]
    public int DeadTreasures;
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
    private List<Hole> holes;
    private int numHoles;
    private bool raining;
    private float rainingCount;
    private bool difficult;
    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        if (difficult = (SavedData.Load<int>("Difficult") != 1))
        {
            WaterRiseRate *= 0.8f;
            SecondsTillRain += 20;
        }
        maxWaterIndicatorSize = WaterIndicator.rectTransform.sizeDelta.x;
        holes = new List<Hole>(FindObjectsOfType<Hole>());
        numHoles = holes.Count;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (WaterValue < 0)
        {
            WaterValue = 0;
        }
        // Holes
        if (holes.Count == 0)
        {
            CurrentLevel.FinishLevel();
        }
        else
        {
            HoleIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * holes.Count / numHoles, WaterIndicator.rectTransform.sizeDelta.y);
            HoleCountIndicator.text = holes.Count.ToString();
        }
        // Rain
        if (!raining)
        {
            rainingCount += Time.deltaTime;
            if (rainingCount >= SecondsTillRain)
            {
                raining = true;
                RainIndicators.ForEach(a => a.SetActive(true));
                rainingCount = SecondsTillRain;
            }
            RainIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * rainingCount / SecondsTillRain, WaterIndicator.rectTransform.sizeDelta.y);
            RainCountIndicator.text = Mathf.RoundToInt(SecondsTillRain - rainingCount).ToString();
        }
        // Treasure
        TreasureIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * (1 - (float)DeadTreasures / TreasureAmount), WaterIndicator.rectTransform.sizeDelta.y);
        TreasureCountIndicator.text = (TreasureAmount - DeadTreasures).ToString();
        // Water
        WaterValue += Time.deltaTime * GetTrueRiseRate();
        WaterIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * WaterValue / MaxWaterValue, WaterIndicator.rectTransform.sizeDelta.y);
        if (WaterValue >= MaxWaterValue)
        {
            SceneManager.LoadScene("Lose");
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
    public void RemoveHole(Hole hole)
    {
        holes.Remove(hole);
        Destroy(hole.gameObject);
    }
    private float GetTrueRiseRate()
    {
        // Reducing water rise rate with the holes makes sense, but also renders throwing treause away pointless. Will need to find a solution.
        return WaterRiseRate * GetTreasureMod() * (0.5f * holes.Count / numHoles + 0.5f) + (raining ? WaterRiseRate * (difficult ? 2 : 1) * GetTreasureMod() : 0);
    }
    private float GetTreasureMod()
    {
        return (1 - DeadTreasures / (float)TreasureAmount) / 2 + 0.5f;
    }
}
