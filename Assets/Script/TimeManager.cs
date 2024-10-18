using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int CurrentHour { get; private set; }
    public int CurrentMinute { get; private set; }

    private float timer; // Таймер для отслеживания времени
    public TextMeshProUGUI timeDisplay; // Текст для отображения времени
    public Light sceneLight; // Освещение на сцене
    public Material dayMaterial, nightMaterial; // Материалы дня и ночи

    public float gameMinutesPerRealSecond = 1f; // 1 секунда = 1 минута в игре
    public int startHour = 7; // Начало дня в 5:00

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentHour = 7; // Начинаем с 7:00
        CurrentMinute = 0;
        UpdateTimeDisplay();
        UpdateLighting();
    }

    private void Update()
    {
        timer += Time.deltaTime * (gameMinutesPerRealSecond * 2); // Увеличиваем скорость в 2 раза

        if (timer >= 60f) // Каждые 60 секунд
        {
            timer = 0f;
            CurrentMinute++;

            if (CurrentMinute >= 60)
            {
                CurrentMinute = 0;
                CurrentHour++;
                if (CurrentHour >= 24) CurrentHour = 0; // Сброс на 0, если прошло 24 часа
            }
        }
        UpdateTimeDisplay();
        UpdateLighting();
    }



    private void UpdateTimeDisplay()
    {
        // Отображаем время в формате HH:mm
        string formattedTime = $"{CurrentHour:D2}:{CurrentMinute:D2}"; // Форматируем время
        timeDisplay.text = $"Время: {formattedTime}";
    }

    private void UpdateLighting()
    {
        // День с 6:00 до 20:00
        bool isDaytime = CurrentHour >= 6 && CurrentHour < 20;

        if (isDaytime)
        {
            sceneLight.intensity = 1.5f;
            RenderSettings.skybox = dayMaterial;
        }
        else
        {
            sceneLight.intensity = 0.3f;
            RenderSettings.skybox = nightMaterial;
        }
    }

    public void SetTime(int hour, int minute)
    {
        CurrentHour = hour;
        CurrentMinute = minute;
        UpdateTimeDisplay(); // Обновляем отображение времени
    }
}
