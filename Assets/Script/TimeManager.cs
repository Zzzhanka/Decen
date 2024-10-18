using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay; // Текст на Canvas
    public Light sceneLight; // Directional Light для изменения освещения
    public Material dayMaterial, nightMaterial; // Материалы для смены дня и ночи

    public float dayDuration = 10f; // Длительность дня в секундах
    private float timeCounter;
    public bool IsDaytime { get; private set; }

    void Start()
    {
        timeCounter = 0;
        UpdateLighting();
    }

    void Update()
    {
        timeCounter += Time.deltaTime;

        // Переключение дня и ночи
        if (timeCounter >= dayDuration)
        {
            IsDaytime = !IsDaytime;
            timeCounter = 0;
            UpdateLighting();
        }

        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        TimeSpan currentTime = TimeSpan.FromSeconds(timeCounter);
        string formattedTime = string.Format("{0:D2}:{1:D2}", currentTime.Minutes, currentTime.Seconds);
        timeDisplay.text = IsDaytime ? $"День: {formattedTime}" : $"Ночь: {formattedTime}";
    }

    private void UpdateLighting()
    {
        if (IsDaytime)
        {
            sceneLight.intensity = 1.5f; // Ярче днём
            RenderSettings.skybox = dayMaterial; // Материал неба для дня
        }
        else
        {
            sceneLight.intensity = 0.3f; // Тускло ночью
            RenderSettings.skybox = nightMaterial; // Материал неба для ночи
        }
    }
}
