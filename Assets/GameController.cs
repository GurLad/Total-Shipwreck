﻿using System.Collections;
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
    [Header("UI")]
    public Image WaterIndicator;
    [HideInInspector]
    public float WaterValue;
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
        WaterObject.HeightMod = WaterValue;
    }
}