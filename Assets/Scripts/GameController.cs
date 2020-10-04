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
    public Image HoleIndicator;
    public Text HoleCountIndicator;
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
    private void Awake()
    {
        Current = this;
    }
    private void Start()
    {
        maxWaterIndicatorSize = WaterIndicator.rectTransform.sizeDelta.x;
        holes = new List<Hole>(FindObjectsOfType<Hole>());
        numHoles = holes.Count;
    }
    private void Update()
    {
        if (holes.Count == 0)
        {
            SceneManager.LoadScene("Win");
        }
        else
        {
            HoleIndicator.rectTransform.sizeDelta = new Vector2(maxWaterIndicatorSize * holes.Count / numHoles, WaterIndicator.rectTransform.sizeDelta.y);
            HoleCountIndicator.text = holes.Count.ToString();
        }
        WaterValue += Time.deltaTime * GetTrueRiseRate();
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
    public void RemoveHole(Hole hole)
    {
        holes.Remove(hole);
        Destroy(hole.gameObject);
    }
    private float GetTrueRiseRate()
    {
        // Reducing water rise rate with the holes makes sense, but also renders throwing treause away pointless. Will need to find a solution.
        return WaterRiseRate * ((float)holes.Count / numHoles);
    }
}
