using System.Collections;
using TMPro;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // ������ �� TextMeshPro ��� ����������� �������
    public int startHour = 7;  // ����� ������ ��� (7 ����)

    private int currentHour;
    private int currentMinute;
    private float realSecondCounter;

    private void Start()
    {
        currentHour = startHour;
        currentMinute = 0;
        realSecondCounter = 0f;
    }

    private void Update()
    {
        // ������� ������� � �������� �������
        realSecondCounter += Time.deltaTime;

        if (realSecondCounter >= 1f)  // ������ ������� (1 ��� = 1 ���)
        {
            realSecondCounter = 0f;
            IncrementGameTime();  // ��������� ������� �����
        }

        // ��������� ����� �� ������
        timeText.text = $"{currentHour:D2}:{currentMinute:D2}";
    }

    private void IncrementGameTime()
    {
        currentMinute++;

        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;

            if (currentHour >= 24)  // ���� ������ 24 ����, �������� � ��������
                currentHour = 0;
        }
    }

    // ����� ��� ��������� �������� ������� � ����
    public int GetCurrentHour() => currentHour;
    public int GetCurrentMinute() => currentMinute;
}
