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

    private float timer; // ������ ��� ������������ �������
    public TextMeshProUGUI timeDisplay; // ����� ��� ����������� �������
    public Light sceneLight; // ��������� �� �����
    public Material dayMaterial, nightMaterial; // ��������� ��� � ����

    public float gameMinutesPerRealSecond = 1f; // 1 ������� = 1 ������ � ����
    public int startHour = 7; // ������ ��� � 5:00

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentHour = 7; // �������� � 7:00
        CurrentMinute = 0;
        UpdateTimeDisplay();
        UpdateLighting();
    }

    private void Update()
    {
        timer += Time.deltaTime * (gameMinutesPerRealSecond * 2); // ����������� �������� � 2 ����

        if (timer >= 60f) // ������ 60 ������
        {
            timer = 0f;
            CurrentMinute++;

            if (CurrentMinute >= 60)
            {
                CurrentMinute = 0;
                CurrentHour++;
                if (CurrentHour >= 24) CurrentHour = 0; // ����� �� 0, ���� ������ 24 ����
            }
        }
        UpdateTimeDisplay();
        UpdateLighting();
    }



    private void UpdateTimeDisplay()
    {
        // ���������� ����� � ������� HH:mm
        string formattedTime = $"{CurrentHour:D2}:{CurrentMinute:D2}"; // ����������� �����
        timeDisplay.text = $"�����: {formattedTime}";
    }

    private void UpdateLighting()
    {
        // ���� � 6:00 �� 20:00
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
        UpdateTimeDisplay(); // ��������� ����������� �������
    }
}
