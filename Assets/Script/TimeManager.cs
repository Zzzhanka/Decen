using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay; // ����� �� Canvas
    public Light sceneLight; // Directional Light ��� ��������� ���������
    public Material dayMaterial, nightMaterial; // ��������� ��� ����� ��� � ����

    public float dayDuration = 10f; // ������������ ��� � ��������
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

        // ������������ ��� � ����
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
        timeDisplay.text = IsDaytime ? $"����: {formattedTime}" : $"����: {formattedTime}";
    }

    private void UpdateLighting()
    {
        if (IsDaytime)
        {
            sceneLight.intensity = 1.5f; // ���� ���
            RenderSettings.skybox = dayMaterial; // �������� ���� ��� ���
        }
        else
        {
            sceneLight.intensity = 0.3f; // ������ �����
            RenderSettings.skybox = nightMaterial; // �������� ���� ��� ����
        }
    }
}
